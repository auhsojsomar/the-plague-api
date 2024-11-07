using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class OrderService : IOrderService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderStatusService _orderStatusService;
    private readonly IPaymentMethodService _paymentMethodService;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUserRepository userRepository,
        IOrderStatusService orderStatusService,
        IPaymentMethodService paymentMethodService)
    {
      _orderRepository = orderRepository;
      _productRepository = productRepository;
      _userRepository = userRepository;
      _orderStatusService = orderStatusService;
      _paymentMethodService = paymentMethodService;
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
      return await _orderRepository.GetAllAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(string id)
    {
      return await _orderRepository.GetByIdAsync(id);
    }

    public async Task<Order> CreateOrderAsync(Order order)
    {
      await ValidateOrderAsync(order);

      // Fetch the Order Status using StatusId to get the StatusKey
      var orderStatus = await GetStatusAsync(order.StatusId);
      var paymentMethod = await GetPaymentMethodAsync(order.PaymentMethodId);

      // If Order Status is Paid or PaymentMethod is Cash on Delivery, update the variant quantity
      if (orderStatus.Key == 2 || paymentMethod.Key == 1)
      {
        foreach (var item in order.Items)
        {
          await UpdateVariantQuantityAsync(item.ProductId, item.VariantId, item.Quantity);
        }
      }

      // Create and return the order
      return await _orderRepository.CreateAsync(order);
    }

    public async Task<bool> UpdateOrderAsync(string id, Order order)
    {
      // Fetch the Order Status using StatusId to get the StatusKey
      var orderStatus = await GetStatusAsync(order.StatusId);

      // If Order Status is Paid, update the variant quantity for each item
      if (orderStatus.Key == 2) // Paid Order Status
      {
        foreach (var item in order.Items)
        {
          await UpdateVariantQuantityAsync(item.ProductId, item.VariantId, item.Quantity);
        }
      }

      // Update and return the result
      return await _orderRepository.UpdateAsync(id, order);
    }

    public async Task<bool> DeleteOrderAsync(string id)
    {
      return await _orderRepository.DeleteAsync(id);
    }

    // Helper method to validate Order details
    private async Task ValidateOrderAsync(Order order)
    {
      await ValidateUserAsync(order.UserId);
      await ValidateItemsAsync(order.Items);
      await ValidateStatusAsync(order.StatusId);
      await ValidatePaymentMethodAsync(order.PaymentMethodId);
    }

    // Helper method to validate each order item
    private async Task ValidateItemsAsync(List<OrderItem> items)
    {
      foreach (var item in items)
      {
        await ValidateProductAndVariantAsync(item.ProductId, item.VariantId, item.Quantity);
      }
    }

    // Helper method to fetch and validate Order Status by ID
    private async Task<OrderStatus> GetStatusAsync(string statusId)
    {
      var orderStatus = await _orderStatusService.GetByIdAsync(statusId);
      if (orderStatus == null)
      {
        throw new ApplicationException("Invalid Order Status.");
      }
      return orderStatus;
    }

    // Helper method to fetch and validate PaymentMethod by ID
    private async Task<PaymentMethod> GetPaymentMethodAsync(string paymentMethodId)
    {
      var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
      if (paymentMethod == null)
      {
        throw new ApplicationException("Invalid PaymentMethod.");
      }
      return paymentMethod;
    }

    // Helper method to validate if UserId is valid
    private async Task ValidateUserAsync(string userId)
    {
      var user = await _userRepository.GetByIdAsync(userId);
      if (user == null)
      {
        throw new ArgumentException("Invalid UserId: User does not exist.");
      }
    }

    // Helper method to validate Order Status
    private async Task ValidateStatusAsync(string statusId)
    {
      var orderStatus = await _orderStatusService.GetByIdAsync(statusId);
      if (orderStatus == null)
      {
        throw new ArgumentException("Invalid Order Status: Order Status does not exist.");
      }
    }

    // Helper method to validate PaymentMethod
    private async Task ValidatePaymentMethodAsync(string paymentMethodId)
    {
      var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
      if (paymentMethod == null)
      {
        throw new ArgumentException("Invalid PaymentMethod: Payment method does not exist.");
      }
    }

    // Helper method to validate if ProductId and VariantId are valid
    private async Task ValidateProductAndVariantAsync(string productId, string variantId, int quantity)
    {
      var product = await _productRepository.GetByIdAsync(productId)
                    ?? throw new ArgumentException("Invalid ProductId: Product not found.");

      var variant = product.Variants.FirstOrDefault(v => v.Id == variantId);
      if (variant == null)
      {
        throw new ArgumentException("Invalid VariantId: Variant does not exist within the specified Product.");
      }

      if (quantity < 1)
      {
        throw new ArgumentException("Quantity must be at least 1.");
      }

      if (quantity > variant.Quantity)
      {
        throw new ArgumentException($"Quantity exceeds the available stock for this variant. Available quantity: {variant.Quantity}.");
      }
    }

    // Method to reduce the variant quantity based on order quantity
    private async Task UpdateVariantQuantityAsync(string productId, string variantId, int orderQuantity)
    {
      var product = await _productRepository.GetByIdAsync(productId)
                    ?? throw new ArgumentException("Invalid ProductId: Product not found.");

      var variant = product.Variants.FirstOrDefault(v => v.Id == variantId);
      if (variant == null)
      {
        throw new ArgumentException("Invalid VariantId: Variant does not exist within the specified Product.");
      }

      if (variant.Quantity < orderQuantity)
      {
        throw new ArgumentException("Order quantity exceeds available stock for the variant.");
      }

      // Update the variant quantity
      variant.Quantity -= orderQuantity;

      // Persist the updated product with the modified variant quantity
      await _productRepository.UpdateAsync(productId, product);
    }
  }
}
