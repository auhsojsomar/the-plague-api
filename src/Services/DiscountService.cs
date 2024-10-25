using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class DiscountService : IDiscountService
  {
    private readonly IDiscountRepository _discountRepository;

    public DiscountService(IDiscountRepository discountRepository)
    {
      _discountRepository = discountRepository;
    }

    public async Task<IEnumerable<Discount>> GetAllDiscountsAsync()
    {
      return await _discountRepository.GetAllAsync();
    }

    public async Task<Discount?> GetDiscountByIdAsync(string id)
    {
      return await _discountRepository.GetByIdAsync(id);
    }

    public async Task<Discount> CreateDiscountAsync(Discount discount)
    {
      return await _discountRepository.CreateAsync(discount);
    }

    public async Task<bool> UpdateDiscountAsync(string id, Discount discount)
    {
      return await _discountRepository.UpdateAsync(id, discount);
    }

    public async Task<bool> DeleteDiscountAsync(string id)
    {
      return await _discountRepository.DeleteAsync(id);
    }
  }
}
