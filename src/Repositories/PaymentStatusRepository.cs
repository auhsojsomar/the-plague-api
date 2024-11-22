using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using MongoDB.Bson;
using The_Plague_Api.Services;
using static The_Plague_Api.Helpers.ValidationHelpers;

namespace The_Plague_Api.Repositories
{
  public class PaymentStatusRepository : IPaymentStatusRepository
  {
    private readonly IMongoDbService<PaymentStatus> _statusService;
    private readonly KeyGeneratorService _keyGeneratorService;
    private readonly IMongoCollection<PaymentStatus> _paymentStatusCollection;

    public PaymentStatusRepository(IMongoDatabase database, KeyGeneratorService keyGeneratorService)
    {
      const string paymentStatusCollection = "paymentStatus";

      _statusService = new MongoDbService<PaymentStatus>(database, paymentStatusCollection);
      _paymentStatusCollection = database.GetCollection<PaymentStatus>(paymentStatusCollection);
      _keyGeneratorService = keyGeneratorService;
    }

    public async Task<IEnumerable<PaymentStatus>> GetAllAsync()
    {
      return await _statusService.GetAllAsync();
    }

    public async Task<PaymentStatus?> GetByIdAsync(string id)
    {
      ValidateId(id);
      return await _statusService.GetAsync(id);
    }

    public async Task<PaymentStatus?> GetByKeyAsync(int key)
    {
      var filter = Builders<PaymentStatus>.Filter.Eq(x => x.Key, key);
      return await _statusService.GetAsync(filter);
    }

    public async Task<PaymentStatus> CreateAsync(PaymentStatus paymentStatus)
    {
      await EnsureStatusNameIsUniqueAsync(paymentStatus.Name);
      paymentStatus.Key = await _keyGeneratorService.GenerateUniqueKeyAsync("paymentStatusKey", _paymentStatusCollection, s => s.Key);
      await _statusService.CreateAsync(paymentStatus);
      return paymentStatus;
    }

    public async Task<bool> UpdateAsync(string id, PaymentStatus paymentStatus)
    {
      ValidateId(id);

      await EnsureStatusNameIsUniqueAsync(paymentStatus.Name, id);

      paymentStatus.Id = id;
      paymentStatus.DateModified = DateTime.UtcNow;
      return await _statusService.UpdateAsync(id, paymentStatus);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      ValidateId(id);
      return await _statusService.DeleteAsync(id);
    }

    // Helper method to ensure name uniqueness
    private async Task EnsureStatusNameIsUniqueAsync(string name, string? excludeId = null)
    {
      var filter = Builders<PaymentStatus>.Filter.Regex(s => s.Name, new BsonRegularExpression($"^{name}$", "i"));
      var existingStatus = await _statusService.GetAsync(filter);

      if (existingStatus != null && existingStatus.Id != excludeId)
      {
        throw new ArgumentException("A payment status with this name already exists.");
      }
    }
  }
}
