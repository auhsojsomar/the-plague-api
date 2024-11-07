using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Helpers;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class CartService : ICartService
  {
    private readonly ICartRepository _cartRepository;
    private readonly IProductRepository _productRepository;

    public CartService(ICartRepository cartRepository, IProductRepository productRepository)
    {
      _cartRepository = cartRepository;
      _productRepository = productRepository;
    }

    public async Task<Cart?> GetCartByIdAsync(string id)
    {
      return await _cartRepository.GetByIdAsync(id);
    }

    public async Task<Cart?> GetCartByUserIdAsync(string userId)
    {
      return await _cartRepository.GetByUserIdAsync(userId);
    }

    public async Task<Cart> CreateCartAsync(Cart cart)
    {
      await ValidateCartItemsAsync(cart.Items);
      return await _cartRepository.CreateAsync(cart);
    }

    public async Task<bool> UpdateCartAsync(string id, Cart cart)
    {
      await ValidateCartItemsAsync(cart.Items);
      return await _cartRepository.UpdateAsync(id, cart);
    }

    public async Task<bool> DeleteCartAsync(string id)
    {
      return await _cartRepository.DeleteAsync(id);
    }

    public async Task<decimal> CalculateTotalPrice(List<CartItem> items)
    {
      decimal total = 0;
      foreach (var item in items)
      {
        var product = await _productRepository.GetByIdAsync(item.ProductId);
        if (product == null) throw new ArgumentException("Product not found.");

        var variant = product.Variants.FirstOrDefault(v => v.Id == item.VariantId);
        if (variant == null) throw new ArgumentException("Variant not found.");

        // Calculate the effective price using PriceHelpers
        var effectivePrice = PriceHelpers.CalculateSalePrice(variant.Price, variant.Discount?.Type, variant.Discount?.Value);
        total += effectivePrice * item.Quantity;
      }
      return total;
    }

    // Helper method to validate cart items
    private async Task ValidateCartItemsAsync(List<CartItem> items)
    {
      foreach (var item in items)
      {
        var product = await _productRepository.GetByIdAsync(item.ProductId)
                      ?? throw new ArgumentException("Invalid ProductId: Product not found.");

        var variant = product.Variants.FirstOrDefault(v => v.Id == item.VariantId);
        if (variant == null)
        {
          throw new ArgumentException("Invalid VariantId: Variant does not exist within the specified Product.");
        }

        if (item.Quantity < 1 || item.Quantity > variant.Quantity)
        {
          throw new ArgumentException("Quantity is invalid or exceeds stock for this variant.");
        }
      }
    }
  }
}
