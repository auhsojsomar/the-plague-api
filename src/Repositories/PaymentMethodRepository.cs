using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Services.Interfaces;
using MongoDB.Bson;
using The_Plague_Api.Services;

namespace The_Plague_Api.Repositories
{
  public class PaymentMethodRepository : IPaymentMethodRepository
  {
    private readonly IMongoDbService<PaymentMethod> _paymentMethodService;

    public PaymentMethodRepository(IMongoDatabase database)
    {
      const string paymentMethodCollection = "paymentMethod";
      _paymentMethodService = new MongoDbService<PaymentMethod>(database, paymentMethodCollection);
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllAsync()
    {
      return await _paymentMethodService.GetAllAsync();
    }

    public async Task<PaymentMethod> GetByIdAsync(string id)
    {
      ValidateId(id);
      return await GetPaymentMethodByIdOrThrowAsync(id);
    }

    public async Task<PaymentMethod> CreateAsync(PaymentMethod paymentMethod)
    {
      await EnsurePaymentMethodNameIsUniqueAsync(paymentMethod.Name);

      paymentMethod.DateCreated = DateTime.UtcNow;
      return await _paymentMethodService.CreateAsync(paymentMethod);
    }

    public async Task<bool> UpdateAsync(string id, PaymentMethod paymentMethod)
    {
      ValidateId(id);

      await EnsurePaymentMethodNameIsUniqueAsync(paymentMethod.Name, id);

      paymentMethod.Id = id;
      paymentMethod.DateModified = DateTime.UtcNow;
      return await _paymentMethodService.UpdateAsync(id, paymentMethod);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      ValidateId(id);
      return await _paymentMethodService.DeleteAsync(id);
    }

    // Helper method to validate ID format
    private static void ValidateId(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        throw new ArgumentException("ID cannot be null or empty.", nameof(id));
    }

    // Helper method to retrieve PaymentMethod by ID or throw exception if not found
    private async Task<PaymentMethod> GetPaymentMethodByIdOrThrowAsync(string id)
    {
      var paymentMethod = await _paymentMethodService.GetAsync(id);
      return paymentMethod ?? throw new KeyNotFoundException($"PaymentMethod with ID '{id}' was not found.");
    }

    // Helper method to ensure name uniqueness
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
