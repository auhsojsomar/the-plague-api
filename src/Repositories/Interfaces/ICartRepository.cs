using The_Plague_Api.Data.Entities.Order;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface ICartRepository
  {
    Task<Cart?> GetByIdAsync(string id);
    Task<Cart?> GetByUserIdAsync(string userId);
    Task<IEnumerable<Cart>> GetAllAsync();
    Task<Cart> CreateAsync(Cart cart);
    Task<bool> UpdateAsync(string id, Cart cart);
    Task<bool> DeleteAsync(string id);
  }
}
