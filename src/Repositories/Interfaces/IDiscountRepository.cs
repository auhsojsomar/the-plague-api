using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IDiscountRepository
  {
    Task<IEnumerable<Discount>> GetAllAsync();
    Task<Discount?> GetByIdAsync(string id);
    Task<Discount> CreateAsync(Discount discount);
    Task<bool> UpdateAsync(string id, Discount discount);
    Task<bool> DeleteAsync(string id);
  }
}
