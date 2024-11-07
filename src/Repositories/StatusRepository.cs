using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Services;
using MongoDB.Bson;
using static The_Plague_Api.Helpers.ValidationHelpers;

namespace The_Plague_Api.Repositories
{
  public class StatusRepository : IStatusRepository
  {
    private readonly IMongoDbService<Status> _statusService;
    private readonly KeyGeneratorService _keyGeneratorService;
    private readonly IMongoCollection<Status> _statusCollection;

    public StatusRepository(IMongoDatabase database, KeyGeneratorService keyGeneratorService)
    {
      const string statusCollection = "status";

      _statusService = new MongoDbService<Status>(database, statusCollection);
      _statusCollection = database.GetCollection<Status>(statusCollection);
      _keyGeneratorService = keyGeneratorService;
    }

    public async Task<IEnumerable<Status>> GetAllAsync()
    {
      return await _statusService.GetAllAsync();
    }

    public async Task<Status?> GetByIdAsync(string id)
    {
      ValidateId(id);
      return await _statusService.GetAsync(id);
    }

    public async Task<Status> CreateAsync(Status status)
    {
      await EnsureStatusNameIsUniqueAsync(status.Name);
      status.Key = await _keyGeneratorService.GenerateUniqueKeyAsync("statusKey", _statusCollection, s => s.Key);
      await _statusService.CreateAsync(status);
      return status;
    }

    public async Task<bool> UpdateAsync(string id, Status status)
    {
      ValidateId(id);

      await EnsureStatusNameIsUniqueAsync(status.Name, id);

      status.Id = id;
      status.DateModified = DateTime.UtcNow;
      return await _statusService.UpdateAsync(id, status);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      ValidateId(id);
      return await _statusService.DeleteAsync(id);
    }


    // Helper method to ensure name uniqueness
    private async Task EnsureStatusNameIsUniqueAsync(string name, string? excludeId = null)
    {
      var filter = Builders<Status>.Filter.Regex(s => s.Name, new BsonRegularExpression($"^{name}$", "i"));
      var existingStatus = await _statusService.GetAsync(filter);

      if (existingStatus != null && existingStatus.Id != excludeId)
      {
        throw new ArgumentException("A status with this name already exists.");
      }
    }

  }
}
