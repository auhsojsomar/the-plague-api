using MongoDB.Driver;
using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using MongoDB.Bson;
using static The_Plague_Api.Helpers.ValidationHelpers;
using The_Plague_Api.Services;

namespace The_Plague_Api.Repositories
{
  public class ShippingFeeRepository : IShippingFeeRepository
  {
    private readonly IMongoDbService<ShippingFee> _shippingFeeService;
    private readonly KeyGeneratorService _keyGeneratorService;
    private readonly IMongoCollection<ShippingFee> _shippingFeeCollection;

    public ShippingFeeRepository(IMongoDatabase database, KeyGeneratorService keyGeneratorService)
    {
      const string shippingFeeCollection = "shippingFee";

      _shippingFeeService = new MongoDbService<ShippingFee>(database, shippingFeeCollection);
      _shippingFeeCollection = database.GetCollection<ShippingFee>(shippingFeeCollection);
      _keyGeneratorService = keyGeneratorService;
    }

    public async Task<IEnumerable<ShippingFee>> GetAllAsync()
    {
      return await _shippingFeeService.GetAllAsync();
    }

    public async Task<ShippingFee?> GetByIdAsync(string id)
    {
      ValidateId(id);
      return await _shippingFeeService.GetAsync(id);
    }

    public async Task<ShippingFee?> GetByNameAsync(string name)
    {
      var filter = Builders<ShippingFee>.Filter.Eq(s => s.Name, name);
      return await _shippingFeeCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<ShippingFee> CreateAsync(ShippingFee shippingFee)
    {
      await EnsureShippingFeeNameIsUniqueAsync(shippingFee.Name);
      shippingFee.Key = await _keyGeneratorService.GenerateUniqueKeyAsync("shippingFeeKey", _shippingFeeCollection, s => s.Key);
      await _shippingFeeService.CreateAsync(shippingFee);
      return shippingFee;
    }

    public async Task<bool> UpdateAsync(string id, ShippingFee shippingFee)
    {
      ValidateId(id);
      await EnsureShippingFeeNameIsUniqueAsync(shippingFee.Name, id);

      shippingFee.Id = id;
      shippingFee.DateModified = DateTime.UtcNow;
      return await _shippingFeeService.UpdateAsync(id, shippingFee);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      ValidateId(id);
      return await _shippingFeeService.DeleteAsync(id);
    }

    // Helper method to ensure name uniqueness
    private async Task EnsureShippingFeeNameIsUniqueAsync(string name, string? excludeId = null)
    {
      var filter = Builders<ShippingFee>.Filter.Regex(s => s.Name, new BsonRegularExpression($"^{name}$", "i"));
      var existingShippingFee = await _shippingFeeService.GetAsync(filter);

      if (existingShippingFee != null && existingShippingFee.Id != excludeId)
      {
        throw new ArgumentException("A shipping fee with this name already exists.");
      }
    }
  }
}
