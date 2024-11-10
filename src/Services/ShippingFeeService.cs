using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class ShippingFeeService : IShippingFeeService
  {
    private readonly IShippingFeeRepository _shippingFeeRepository;

    public ShippingFeeService(IShippingFeeRepository shippingFeeRepository)
    {
      _shippingFeeRepository = shippingFeeRepository;
    }

    public async Task<IEnumerable<ShippingFee>> GetAllShippingFeesAsync()
    {
      return await _shippingFeeRepository.GetAllAsync();
    }

    public async Task<ShippingFee?> GetShippingFeeByIdAsync(string id)
    {
      return await _shippingFeeRepository.GetByIdAsync(id);
    }

    public async Task<ShippingFee> CreateShippingFeeAsync(ShippingFee shippingFee)
    {
      return await _shippingFeeRepository.CreateAsync(shippingFee);
    }

    public async Task<bool> UpdateShippingFeeAsync(string id, ShippingFee shippingFee)
    {
      return await _shippingFeeRepository.UpdateAsync(id, shippingFee);
    }

    public async Task<bool> DeleteShippingFeeAsync(string id)
    {
      return await _shippingFeeRepository.DeleteAsync(id);
    }
  }
}
