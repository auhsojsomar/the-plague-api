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

    public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
    {
      _orderRepository = orderRepository;
      _productRepository = productRepository;
      _userRepository = userRepository;
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
      await ValidateUserAsync(order.UserId);
      await ValidateProductAndVariantAsync(order.ProductId, order.VariantId, order.Quantity);

      order.DateCreated = DateTime.UtcNow;
      return await _orderRepository.CreateAsync(order);
    }

    public async Task<bool> UpdateOrderAsync(string id, Order order)
    {
      return await _orderRepository.UpdateAsync(id, order);
    }

    public async Task<bool> DeleteOrderAsync(string id)
    {
      return await _orderRepository.DeleteAsync(id);
    }

    // Helper method to validate if UserId is valid
    private async Task ValidateUserAsync(string userId)
    {
      var user = await _userRepository.GetByIdAsync(userId); // Assume ExistsAsync is a method in IUserRepository
      if (user == null)
      {
        throw new ArgumentException("Invalid UserId: User does not exist.");
      }
    }

    // Helper method to validate if ProductId and VariantId are valid
    private async Task ValidateProductAndVariantAsync(string productId, string variantId, int quantity)
    {
      // Retrieve the product from the repository
      var product = await _productRepository.GetByIdAsync(productId)
                      ?? throw new ArgumentException("Invalid ProductId: Product not found.");

      // Find the variant within the product
      var variant = product.Variants.FirstOrDefault(v => v.Id == variantId);
      if (variant == null)
      {
        throw new ArgumentException("Invalid VariantId: Variant does not exist within the specified Product.");
      }

      // Validate quantity
      if (quantity < 1)
      {
        throw new ArgumentException("Quantity must be at least 1.");
      }

      if (quantity > variant.Quantity)
      {
        throw new ArgumentException($"Quantity exceeds the available stock for this variant. Available quantity: {variant.Quantity}.");
      }
    }
  }
}
