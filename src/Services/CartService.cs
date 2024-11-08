#nullable enable

using The_Plague_Api.Data.Entities.Order;
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
      _cartRepository = cartRepository ?? throw new ArgumentNullException(nameof(cartRepository));
      _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<Cart?> GetCartByIdAsync(string id)
    {
      ValidateStringNotNullOrEmpty(id, nameof(id));
      return await _cartRepository.GetByIdAsync(id);
    }

    public async Task<Cart?> GetCartByUserIdAsync(string userId)
    {
      ValidateStringNotNullOrEmpty(userId, nameof(userId));
      return await _cartRepository.GetByUserIdAsync(userId);
    }

    public async Task<Cart> CreateCartAsync(Cart cart)
    {
      if (cart.Items == null || !cart.Items.Any())
      {
        throw new ArgumentException("Cart must have at least one valid item.");
      }

      // Check if the cart already exists for the user
      var existingCart = await _cartRepository.GetByUserIdAsync(cart.UserId);
      return existingCart != null
          ? await UpdateExistingCartAsync(existingCart, cart)
          : await CreateNewCartAsync(cart);
    }

    public async Task<bool> UpdateCartAsync(string id, Cart cart)
    {
      ValidateStringNotNullOrEmpty(id, nameof(id));
      return await _cartRepository.UpdateAsync(id, cart);
    }

    public async Task<bool> DeleteCartAsync(string id)
    {
      ValidateStringNotNullOrEmpty(id, nameof(id));
      return await _cartRepository.DeleteAsync(id);
    }

    private async Task<Cart> UpdateExistingCartAsync(Cart existingCart, Cart cart)
    {
      // Check if the cart ID is null
      if (string.IsNullOrEmpty(existingCart.Id))
      {
        throw new ArgumentException("Cart ID cannot be null or empty.", nameof(existingCart.Id));
      }

      foreach (var item in cart.Items)
      {
        await AddOrUpdateItemInCartAsync(existingCart, item.ProductId, item.VariantId, item.Quantity);
      }

      // Persist the updated cart
      await _cartRepository.UpdateAsync(existingCart.Id, existingCart);
      return existingCart;
    }

    private async Task<Cart> CreateNewCartAsync(Cart cart)
    {
      await _cartRepository.CreateAsync(cart);
      return cart;
    }

    public async Task<decimal> CalculateTotalPrice(List<CartItem> items)
    {
      if (items == null || !items.Any())
        throw new ArgumentException("Items list cannot be null or empty.", nameof(items));

      decimal totalPrice = 0;
      foreach (var item in items)
      {
        var product = await _productRepository.GetByIdAsync(item.ProductId);
        if (product != null)
        {
          var variant = product.Variants.FirstOrDefault(v => v.Id == item.VariantId);
          if (variant != null)
          {
            totalPrice += variant.Price * item.Quantity;
          }
        }
      }

      return totalPrice;
    }

    public async Task<bool> ClearCartItemsAsync(string userId)
    {
      ValidateStringNotNullOrEmpty(userId, nameof(userId));

      var cart = await _cartRepository.GetByUserIdAsync(userId);
      if (cart == null) return false;

      // Check if the cart ID is null
      if (string.IsNullOrEmpty(cart.Id))
      {
        throw new ArgumentException("Cart ID cannot be null or empty.", nameof(cart.Id));
      }

      cart.Items.Clear();
      cart.DateModified = DateTime.UtcNow;
      await _cartRepository.UpdateAsync(cart.Id, cart);
      return true;
    }

    // Add or Update Item in Cart
    public async Task<Cart> AddOrUpdateItemInCartAsync(Cart cart, string productId, string variantId, int quantity)
    {
      // Check if the cart ID is null
      if (string.IsNullOrEmpty(cart.Id))
      {
        throw new ArgumentException("Cart ID cannot be null or empty.", nameof(cart.Id));
      }

      ValidateStringNotNullOrEmpty(productId, nameof(productId));
      ValidateStringNotNullOrEmpty(variantId, nameof(variantId));

      var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == productId && item.VariantId == variantId);
      if (existingItem != null)
      {
        // Update quantity if item exists
        existingItem.Quantity += quantity;
      }
      else
      {
        // Add new item to cart if it does not exist
        cart.Items.Add(new CartItem
        {
          ProductId = productId,
          VariantId = variantId,
          Quantity = quantity
        });
      }

      // Update the cart's DateModified field
      cart.DateModified = DateTime.UtcNow;
      await _cartRepository.UpdateAsync(cart.Id, cart);

      return cart;
    }

    public async Task<bool> RemoveOrderedItemsFromCartAsync(string userId, List<OrderItem> orderedItems)
    {
      ValidateStringNotNullOrEmpty(userId, nameof(userId));

      if (orderedItems == null || !orderedItems.Any())
        throw new ArgumentException("Ordered items list cannot be null or empty.", nameof(orderedItems));

      var cart = await _cartRepository.GetByUserIdAsync(userId);
      if (cart == null) return false;

      // Check if the cart ID is null
      if (string.IsNullOrEmpty(cart.Id))
      {
        throw new ArgumentException("Cart ID cannot be null or empty.", nameof(cart.Id));
      }

      foreach (var orderedItem in orderedItems)
      {
        var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == orderedItem.ProductId && item.VariantId == orderedItem.VariantId);
        if (cartItem != null)
        {
          cart.Items.Remove(cartItem);
        }
      }

      cart.DateModified = DateTime.UtcNow;
      await _cartRepository.UpdateAsync(cart.Id, cart);
      return true;
    }

    // Method to validate if a string is null or empty
    private void ValidateStringNotNullOrEmpty(string value, string paramName)
    {
      if (string.IsNullOrEmpty(value)) throw new ArgumentException($"{paramName} cannot be null or empty.", paramName);
    }
  }
}
