using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Services;
using MongoDB.Bson;
using static The_Plague_Api.Helpers.ValidationHelpers;

namespace The_Plague_Api.Repositories
{
  public class OrderStatusRepository : IOrderStatusRepository
  {
    private readonly IMongoDbService<OrderStatus> _statusService;
    private readonly KeyGeneratorService _keyGeneratorService;
    private readonly IMongoCollection<OrderStatus> _orderStatusCollection;

    public OrderStatusRepository(IMongoDatabase database, KeyGeneratorService keyGeneratorService)
    {
      const string statusCollection = "orderStatus";

      _statusService = new MongoDbService<OrderStatus>(database, statusCollection);
      _orderStatusCollection = database.GetCollection<OrderStatus>(statusCollection);
      _keyGeneratorService = keyGeneratorService;
    }

    public async Task<IEnumerable<OrderStatus>> GetAllAsync()
    {
      return await _statusService.GetAllAsync();
    }

    public async Task<OrderStatus?> GetByIdAsync(string id)
    {
      ValidateId(id);
      return await _statusService.GetAsync(id);
    }

    public async Task<OrderStatus> CreateAsync(OrderStatus orderStatus)
    {
      await EnsureStatusNameIsUniqueAsync(orderStatus.Name);
      orderStatus.Key = await _keyGeneratorService.GenerateUniqueKeyAsync("orderStatusKey", _orderStatusCollection, s => s.Key);
      await _statusService.CreateAsync(orderStatus);
      return orderStatus;
    }

    public async Task<bool> UpdateAsync(string id, OrderStatus orderStatus)
    {
      ValidateId(id);

      await EnsureStatusNameIsUniqueAsync(orderStatus.Name, id);

      orderStatus.Id = id;
      orderStatus.DateModified = DateTime.UtcNow;
      return await _statusService.UpdateAsync(id, orderStatus);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      ValidateId(id);
      return await _statusService.DeleteAsync(id);
    }


    // Helper method to ensure name uniqueness
    private async Task EnsureStatusNameIsUniqueAsync(string name, string? excludeId = null)
    {
      var filter = Builders<OrderStatus>.Filter.Regex(s => s.Name, new BsonRegularExpression($"^{name}$", "i"));
      var existingStatus = await _statusService.GetAsync(filter);

      if (existingStatus != null && existingStatus.Id != excludeId)
      {
        throw new ArgumentException("A order status with this name already exists.");
      }
    }

  }
}
