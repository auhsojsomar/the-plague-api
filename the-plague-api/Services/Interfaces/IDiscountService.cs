using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IDiscountService
  {
    Task<Discount> CreateDiscountAsync(Discount discount);
    Task<Discount?> GetDiscountByIdAsync(string id);
    Task<IEnumerable<Discount>> GetAllDiscountsAsync();
    Task<bool> DeleteDiscountAsync(string id);
  }
}
