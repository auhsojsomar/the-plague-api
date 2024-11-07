using The_Plague_Api.Data.Entities.Order;

namespace The_Plague_Api.Services.Interfaces
{
  public interface ICartService
  {
    Task<Cart?> GetCartByIdAsync(string id);
    Task<Cart?> GetCartByUserIdAsync(string userId);
    Task<Cart> CreateCartAsync(Cart cart);
    Task<bool> UpdateCartAsync(string id, Cart cart);
    Task<bool> DeleteCartAsync(string id);
    Task<decimal> CalculateTotalPrice(List<CartItem> items);
  }
}
