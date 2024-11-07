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
    private readonly IStatusService _statusService;
    private readonly IPaymentMethodService _paymentMethodService;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        IUserRepository userRepository,
        IStatusService statusService,
        IPaymentMethodService paymentMethodService)
    {
      _orderRepository = orderRepository;
      _productRepository = productRepository;
      _userRepository = userRepository;
      _statusService = statusService;
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

      // Fetch the Status using StatusId to get the StatusKey
      var status = await GetStatusAsync(order.StatusId);
      var paymentMethod = await GetPaymentMethodAsync(order.PaymentMethodId);

      // If Status is Paid, update the variant quantity
      // Update the product quantity when the payment method is Cash on Delivery or the status is Paid
      if (status.Key == 2 || paymentMethod.Key == 1)
      {
        await UpdateVariantQuantityAsync(order.ProductId, order.VariantId, order.Quantity);
      }

      // Create and return the order
      return await _orderRepository.CreateAsync(order);
    }

    public async Task<bool> UpdateOrderAsync(string id, Order order)
    {
      // Fetch the Status using StatusId to get the StatusKey
      var status = await GetStatusAsync(order.StatusId);

      // If Status is Paid, update the variant quantity
      if (status.Key == 2) // Paid Status
      {
        await UpdateVariantQuantityAsync(order.ProductId, order.VariantId, order.Quantity);
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
      await ValidateProductAndVariantAsync(order.ProductId, order.VariantId, order.Quantity);
      await ValidateStatusAsync(order.StatusId);
      await ValidatePaymentMethodAsync(order.PaymentMethodId);
    }

    // Helper method to fetch and validate Status by ID
    private async Task<Status> GetStatusAsync(string statusId)
    {
      var status = await _statusService.GetByIdAsync(statusId);
      if (status == null)
      {
        throw new ApplicationException("Invalid Status.");
      }
      return status;
    }

    // Helper method to fetch and validate Status by ID
    private async Task<PaymentMethod> GetPaymentMethodAsync(string paymentMethodId)
    {
      var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
      if (paymentMethod == null)
      {
        throw new ApplicationException("Invalid Status.");
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

    // Helper method to validate StatusKey is valid
    private async Task ValidateStatusAsync(string statusId)
    {
      var status = await _statusService.GetByIdAsync(statusId);
      if (status == null)
      {
        throw new ArgumentException("Invalid StatusKey: Status does not exist.");
      }
    }

    // Helper method to validate PaymentMethodId is valid
    private async Task ValidatePaymentMethodAsync(string paymentMethodId)
    {
      var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId);
      if (paymentMethod == null)
      {
        throw new ArgumentException("Invalid PaymentMethodId: Payment method does not exist.");
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
