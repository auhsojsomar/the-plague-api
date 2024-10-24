using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IDiscountRepository
  {
    Task<Discount> CreateAsync(Discount discount);
    Task<Discount?> GetByIdAsync(string id);
    Task<IEnumerable<Discount>> GetAllAsync();
    Task<bool> DeleteAsync(string id);
  }
}
