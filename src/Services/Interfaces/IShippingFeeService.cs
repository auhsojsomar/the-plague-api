using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IShippingFeeService
  {
    Task<IEnumerable<ShippingFee>> GetAllShippingFeesAsync();
    Task<ShippingFee?> GetShippingFeeByIdAsync(string id);
    Task<ShippingFee?> GetShippingFeeByNameAsync(string name);
    Task<ShippingFee> CreateShippingFeeAsync(ShippingFee shippingFee);
    Task<bool> UpdateShippingFeeAsync(string id, ShippingFee shippingFee);
    Task<bool> DeleteShippingFeeAsync(string id);
  }
}
