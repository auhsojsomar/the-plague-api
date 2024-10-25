using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IDiscountService
  {
    Task<IEnumerable<Discount>> GetAllDiscountsAsync();
    Task<Discount?> GetDiscountByIdAsync(string id);
    Task<Discount> CreateDiscountAsync(Discount discount);
    Task<bool> UpdateDiscountAsync(string id, Discount discount);
    Task<bool> DeleteDiscountAsync(string id);
  }
}
