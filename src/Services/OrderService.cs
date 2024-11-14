using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using static The_Plague_Api.Helpers.ValidationHelpers;
using static The_Plague_Api.Helpers.StockHelpers;
using AutoMapper;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.Product;

namespace The_Plague_Api.Services
{
  public class OrderService : IOrderService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPaymentStatusRepository _paymentStatusRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IOrderStatusRepository _orderStatusRepository;
    private readonly IShippingFeeRepository _shippingFeeRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IPaymentStatusRepository paymentStatusRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IOrderStatusRepository orderStatusRepository,
        IShippingFeeRepository shippingFeeRepository,
        IUserRepository userRepository,
        ICartService cartService,
        IMapper mapper)
    {
      _orderRepository = orderRepository;
      _productRepository = productRepository;
      _paymentStatusRepository = paymentStatusRepository;
      _paymentMethodRepository = paymentMethodRepository;
      _orderStatusRepository = orderStatusRepository;
      _shippingFeeRepository = shippingFeeRepository;
      _userRepository = userRepository;
      _cartService = cartService;
      _mapper = mapper;
    }

    public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
    {
      var orders = await _orderRepository.GetAllAsync();
      var orderDtos = new List<OrderDto>();

      foreach (var order in orders)
      {
        var orderDto = await MapOrderToDtoAsync(order);
        orderDtos.Add(orderDto);
      }

      return orderDtos;
    }

    public async Task<OrderDto> CreateOrderAsync(Order order)
    {
      // Validate and prepare order
      await ValidateEntityIdsAsync(order);
      await UpdateTotalPriceAsync(order);
      await UpdateStockAndRemoveFromCartOnPayment(order);

      // Handle UserId for guest or existing user
      if (string.IsNullOrEmpty(order.UserId) || order.UserId.StartsWith("Guest"))
      {
        order.UserId = GenerateGuestId();
      }
      else
      {
        await AssignUserDtoToOrderAsync(order);
      }

      // Persist order
      var createdOrder = await _orderRepository.CreateAsync(order);

      // Map the order to DTO and fetch related entities
      var orderDto = await MapOrderToDtoAsync(createdOrder);
      return orderDto;
    }

    public async Task<bool> UpdateOrderAsync(string id, Order order)
    {
      ValidateId(id);
      await ValidateEntityIdsAsync(order);
      await UpdateTotalPriceAsync(order);
      await UpdateStockAndRemoveFromCartOnPayment(order);
      return await _orderRepository.UpdateAsync(id, order);
    }

    public async Task<bool> DeleteOrderAsync(string id)
    {
      ValidateId(id);
      return await _orderRepository.DeleteAsync(id);
    }

    // Add back the GetOrderByIdAsync method
    public async Task<Order?> GetOrderByIdAsync(string id)
    {
      ValidateId(id);
      return await _orderRepository.GetByIdAsync(id);
    }

    private async Task<OrderDto> MapOrderToDtoAsync(Order order)
    {
      var orderDto = _mapper.Map<OrderDto>(order);

      // Fetch related entities and populate DTO fields
      var orderStatus = await _orderStatusRepository.GetByKeyAsync(order.OrderStatusKey);
      var paymentMethod = await _paymentMethodRepository.GetByKeyAsync(order.PaymentMethodKey);
      var paymentStatus = await _paymentStatusRepository.GetByKeyAsync(order.PaymentStatusKey);
      var shippingFee = await _shippingFeeRepository.GetByKeyAsync(order.ShippingFeeKey);

      orderDto.OrderStatus = _mapper.Map<BaseDto>(orderStatus);
      orderDto.PaymentMethod = _mapper.Map<BaseDto>(paymentMethod);
      orderDto.PaymentStatus = _mapper.Map<BaseDto>(paymentStatus);
      orderDto.ShippingFee = _mapper.Map<ShippingFeeDto>(shippingFee);

      // Assign user details
      if (order.UserId != null)
      {
        orderDto.User = await GetUserByIdOrGuestAsync(order.UserId);
      }

      // Map each item to include Product name and formatted Variant
      orderDto.Items = new List<OrderItemDto>();

      foreach (var item in order.Items)
      {
        // Fetch product and check if it exists
        var product = await _productRepository.GetByIdAsync(item.ProductId);
        if (product != null)
        {
          var orderItemDto = new OrderItemDto
          {
            Product = product.Name, // Set Product name

            // Look for the variant within the product's variants list
            Variant = product.Variants.FirstOrDefault(v => v.Id == item.VariantId) is Variant variant
                  ? $"{variant.Color.Name}, {variant.Size.Name}" // Format Variant as "Color, Size"
                  : throw new ArgumentException($"Variant with ID {item.VariantId} not found in Product {item.ProductId}."),

            Quantity = item.Quantity
          };

          orderDto.Items.Add(orderItemDto);
        }
        else
        {
          throw new ArgumentException($"Product with ID {item.ProductId} not found.");
        }
      }

      return orderDto;
    }

    private async Task<UserDto?> GetUserByIdOrGuestAsync(string userId)
    {
      if (userId.StartsWith("Guest"))
      {
        // Return a GuestDto if the UserId starts with "Guest"
        return _mapper.Map<UserDto>(new GuestDto { Id = userId });
      }

      // Otherwise, retrieve and map the actual UserDto
      var user = await _userRepository.GetByIdAsync(userId);

      if (user == null)
      {
        throw new ApplicationException($"User with ID {userId} not found.");
      }

      return _mapper.Map<UserDto>(user);
    }

    private async Task AssignUserDtoToOrderAsync(Order order)
    {
      if (order.UserId != null)
      {
        var user = await _userRepository.GetByIdAsync(order.UserId);
        var userDto = _mapper.Map<UserDto>(user);
        order.UserId = userDto.Id;
      }
    }

    private string GenerateGuestId()
    {
      return $"Guest{Guid.NewGuid().ToString("N").Substring(0, 8)}";
    }

    private async Task ValidateEntityIdsAsync(Order order)
    {
      foreach (var item in order.Items)
      {
        await ValidateProductAndVariantAsync(item.ProductId, item.VariantId, _productRepository);
      }
      await ValidateEntityExistenceByKeyAsync(order.OrderStatusKey, _orderStatusRepository, "OrderStatus");
      await ValidateEntityExistenceByKeyAsync(order.PaymentMethodKey, _paymentMethodRepository, "PaymentMethod");
      await ValidateEntityExistenceByKeyAsync(order.PaymentStatusKey, _paymentStatusRepository, "PaymentStatus");
    }

    private async Task UpdateStockAndRemoveFromCartOnPayment(Order order)
    {
      if (order.PaymentStatusKey == 2) // Assuming 2 is the 'Paid' status
      {
        foreach (var item in order.Items)
        {
          await UpdateVariantQuantityAsync(item.ProductId, item.VariantId, item.Quantity, _productRepository);
        }

        // Remove only the ordered items from the user's cart
        if (order.UserId != null) await _cartService.RemoveOrderedItemsFromCartAsync(order.UserId, order.Items);
      }
    }

    public async Task<decimal> CalculateFee(Order order)
    {
      decimal subTotal = 0;
      var shippingFee = await _shippingFeeRepository.GetByKeyAsync(order.ShippingFeeKey);

      foreach (var item in order.Items)
      {
        var product = await _productRepository.GetByIdAsync(item.ProductId);
        var variant = product?.Variants.FirstOrDefault(v => v.Id == item.VariantId);

        if (variant == null) throw new ArgumentException($"Invalid VariantId: Variant not found.");

        var variantDto = _mapper.Map<VariantDto>(variant);
        decimal itemPrice = variantDto.SalePrice ?? variantDto.Price;
        subTotal += itemPrice * item.Quantity;
      }

      return subTotal + (shippingFee?.Cost ?? 0);
    }

    public async Task UpdateTotalPriceAsync(Order order)
    {
      decimal? totalPrice = await CalculateFee(order);
      order.SetTotalPrice(totalPrice);
    }
  }
}
