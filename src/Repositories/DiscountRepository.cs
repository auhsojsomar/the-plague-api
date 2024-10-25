using MongoDB.Bson;
using MongoDB.Driver;
using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Services;

namespace The_Plague_Api.Repositories
{
  public class DiscountRepository : IDiscountRepository
  {
    private readonly IMongoDbService<Discount> _discountService;

    public DiscountRepository(IMongoDatabase database)
    {
      const string discountCollection = "discounts";
      _discountService = new MongoDbService<Discount>(database, discountCollection);
    }

    public async Task<IEnumerable<Discount>> GetAllAsync()
    {
      return await _discountService.GetAllAsync();
    }

    public async Task<Discount?> GetByIdAsync(string id)
    {
      return await _discountService.GetAsync(id);
    }

    public async Task<Discount> CreateAsync(Discount discount)
    {
      discount.Id = ObjectId.GenerateNewId().ToString();
      return await _discountService.CreateAsync(discount);
    }

    public async Task<bool> UpdateAsync(string id, Discount updatedDiscount)
    {
      return await _discountService.UpdateAsync(id, updatedDiscount);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      return await _discountService.DeleteAsync(id);
    }
  }
}
