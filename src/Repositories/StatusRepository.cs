using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Services;
using MongoDB.Bson;

namespace The_Plague_Api.Repositories
{
  public class StatusRepository : IStatusRepository
  {
    private readonly IMongoDbService<Status> _statusService;

    public StatusRepository(IMongoDatabase database)
    {
      const string statusCollection = "status";
      _statusService = new MongoDbService<Status>(database, statusCollection);
    }

    public async Task<IEnumerable<Status>> GetAllAsync()
    {
      return await _statusService.GetAllAsync();
    }

    public async Task<Status> GetByIdAsync(string id)
    {
      ValidateId(id);
      return await GetStatusByIdOrThrowAsync(id);
    }

    public async Task<Status> CreateAsync(Status status)
    {
      await EnsureStatusNameIsUniqueAsync(status.Name);

      status.DateCreated = DateTime.UtcNow;
      return await _statusService.CreateAsync(status);
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

    // Helper method to validate ID format
    private static void ValidateId(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("ID cannot be null or empty.", nameof(id));
    }

    // Helper method to retrieve status by ID or throw exception if not found
    private async Task<Status> GetStatusByIdOrThrowAsync(string id)
    {
      var status = await _statusService.GetAsync(id);
      return status ?? throw new KeyNotFoundException($"Status with ID '{id}' was not found.");
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
