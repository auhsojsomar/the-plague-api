using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IShippingFeeRepository
  {
    Task<IEnumerable<ShippingFee>> GetAllAsync();
    Task<ShippingFee?> GetByIdAsync(string id);
    Task<ShippingFee> CreateAsync(ShippingFee shippingFee);
    Task<bool> UpdateAsync(string id, ShippingFee shippingFee);
    Task<bool> DeleteAsync(string id);
  }
}
