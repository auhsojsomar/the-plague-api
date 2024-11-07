using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Order;
using MongoDB.Bson;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Services;

namespace The_Plague_Api.Repositories
{
  public class PaymentMethodRepository : IPaymentMethodRepository
  {
    private readonly IMongoDbService<PaymentMethod> _paymentMethodService;
    private readonly IMongoCollection<BsonDocument> _counterCollection;

    public PaymentMethodRepository(IMongoDatabase database)
    {
      const string paymentMethodCollection = "paymentMethod";
      const string counterCollection = "counter";

      _paymentMethodService = new MongoDbService<PaymentMethod>(database, paymentMethodCollection);
      _counterCollection = database.GetCollection<BsonDocument>(counterCollection);
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
      return await _paymentMethodService.GetAllAsync();
    }

    public async Task<PaymentMethod?> GetByIdAsync(string id)
    {
      return await _paymentMethodService.GetAsync(id);
    }

    public async Task<PaymentMethod?> GetByNameAsync(string name)
    {
      var filter = Builders<PaymentMethod>.Filter.Regex(p => p.Name, new BsonRegularExpression($"^{name}$", "i"));
      return await _paymentMethodService.GetAsync(filter);
    }

    public async Task<PaymentMethod> CreateAsync(PaymentMethod paymentMethod)
    {
      await EnsurePaymentMethodNameIsUniqueAsync(paymentMethod.Name);
      paymentMethod.Key = await GetNextPaymentMethodKeyAsync();
      await _paymentMethodService.CreateAsync(paymentMethod);
      return paymentMethod;
    }

    public async Task<bool> UpdateAsync(string id, PaymentMethod paymentMethod)
    {
      paymentMethod.Id = id;
      await EnsurePaymentMethodNameIsUniqueAsync(paymentMethod.Name, id);
      return await _paymentMethodService.UpdateAsync(id, paymentMethod);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      return await _paymentMethodService.DeleteAsync(id);
    }

    // Helper to ensure name uniqueness
    private async Task EnsurePaymentMethodNameIsUniqueAsync(string name, string? excludeId = null)
    {
      var filter = Builders<PaymentMethod>.Filter.Regex(p => p.Name, new BsonRegularExpression($"^{name}$", "i"));
      var existingPaymentMethod = await _paymentMethodService.GetAsync(filter);

      if (existingPaymentMethod != null && existingPaymentMethod.Id != excludeId)
      {
        throw new ArgumentException("A payment method with this name already exists.");
      }
    }

    // Helper to generate a unique key
    private async Task<int> GetNextPaymentMethodKeyAsync()
    {
      var filter = Builders<BsonDocument>.Filter.Eq("_id", "paymentMethodKey");
      var update = Builders<BsonDocument>.Update.Inc("sequence_value", 1);
      var options = new FindOneAndUpdateOptions<BsonDocument>
      {
        IsUpsert = true,
        ReturnDocument = ReturnDocument.After
      };

      var result = await _counterCollection.FindOneAndUpdateAsync(filter, update, options);
      int nextKey = result["sequence_value"].AsInt32;

      var paymentMethodFilter = Builders<PaymentMethod>.Filter.Eq(p => p.Key, nextKey);
      var existingPaymentMethod = await _paymentMethodService.GetAsync(paymentMethodFilter);

      if (existingPaymentMethod != null)
      {
        throw new InvalidOperationException($"The generated key {nextKey} already exists.");
      }

      return nextKey;
    }
  }
}
