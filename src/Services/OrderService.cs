using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using static The_Plague_Api.Helpers.ValidationHelpers;
using static The_Plague_Api.Helpers.StockHelpers;
using AutoMapper;
using The_Plague_Api.Data.Dto;

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
    private readonly ICartService _cartService;
    private readonly IMapper _mapper;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IPaymentStatusRepository paymentStatusRepository,
        IPaymentMethodRepository paymentMethodRepository,
        IOrderStatusRepository orderStatusRepository,
        IShippingFeeRepository shippingFeeRepository,
        ICartService cartService,
        IMapper mapper)
    {
      _orderRepository = orderRepository;
      _productRepository = productRepository;
      _paymentStatusRepository = paymentStatusRepository;
      _paymentMethodRepository = paymentMethodRepository;
      _orderStatusRepository = orderStatusRepository;
      _shippingFeeRepository = shippingFeeRepository;
      _cartService = cartService;
      _mapper = mapper;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
      return await _orderRepository.GetAllAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
      ValidateId(id);
      return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
      // Validate entity IDs (Products, Variants, etc.)
      await ValidateEntityIdsAsync(order);

      // Calculate the total fee (including the subtotal and shipping fee)
      await UpdateTotalPriceAsync(order);

      // Update stock and remove from the cart on payment
      await UpdateStockAndRemoveFromCartOnPayment(order);

      // Create the order and persist it to the repository
      return await _orderRepository.CreateAsync(order);
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
        // Retrieve the product by ProductId
        var product = await _productRepository.GetByIdAsync(item.ProductId);
        if (product == null)
        {
          throw new ArgumentException($"Invalid ProductId: Product not found.");
        }

        // Retrieve the variant by VariantId
        var variant = product.Variants.FirstOrDefault(v => v.Id == item.VariantId);
        if (variant == null)
        {
          throw new ArgumentException($"Invalid VariantId: Variant not found.");
        }

        // Use AutoMapper to map Variant to VariantDto
        var variantDto = _mapper.Map<VariantDto>(variant);

        // Use SalePrice from VariantDto if available
        decimal itemPrice = variantDto.SalePrice ?? variantDto.Price; // Use SalePrice if available, otherwise use the Price

        // Calculate the total price for this order item
        subTotal += itemPrice * item.Quantity;
      }

      if (shippingFee != null)
      {
        return subTotal + shippingFee.Cost;
      }
      return subTotal;
    }

    public async Task UpdateTotalPriceAsync(Order order)
    {
      decimal? totalPrice = await CalculateFee(order);
      order.SetTotalPrice(totalPrice);
    }
  }
}
