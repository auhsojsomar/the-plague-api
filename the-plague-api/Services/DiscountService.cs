using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class DiscountService : IDiscountService
  {
    private readonly IDiscountRepository _repository;

    public DiscountService(IDiscountRepository repository)
    {
      _repository = repository;
    }

    public async Task<Discount> CreateDiscountAsync(Discount discount)
    {
      return await _repository.CreateAsync(discount);
    }

    public async Task<Discount?> GetDiscountByIdAsync(string id)
    {
      return await _repository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Discount>> GetAllDiscountsAsync()
    {
      return await _repository.GetAllAsync();
    }

    public async Task<bool> DeleteDiscountAsync(string id)
    {
      return await _repository.DeleteAsync(id);
    }
  }
}
