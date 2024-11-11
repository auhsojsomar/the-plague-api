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
    private readonly KeyGeneratorService _keyGeneratorService;
    private readonly IMongoCollection<PaymentMethod> _paymentMethodCollection;

    public PaymentMethodRepository(IMongoDatabase database, KeyGeneratorService keyGeneratorService)
    {
      const string paymentMethodCollection = "paymentMethod";

      _paymentMethodService = new MongoDbService<PaymentMethod>(database, paymentMethodCollection);
      _paymentMethodCollection = database.GetCollection<PaymentMethod>(paymentMethodCollection);
      _keyGeneratorService = keyGeneratorService;

    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
      return await _paymentMethodService.GetAllAsync();
    }

    public async Task<PaymentMethod?> GetByIdAsync(string id)
    {
      return await _paymentMethodService.GetAsync(id);
    }

    public async Task<PaymentMethod?> GetByKeyAsync(int key)
    {
      var filter = Builders<PaymentMethod>.Filter.Eq(x => x.Key, key);
      return await _paymentMethodService.GetAsync(filter);
    }

    public async Task<PaymentMethod> CreateAsync(PaymentMethod paymentMethod)
    {
      await EnsurePaymentMethodNameIsUniqueAsync(paymentMethod.Name);
      paymentMethod.Key = await _keyGeneratorService.GenerateUniqueKeyAsync("paymentMethodKey", _paymentMethodCollection, p => p.Key);
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

  }
}
