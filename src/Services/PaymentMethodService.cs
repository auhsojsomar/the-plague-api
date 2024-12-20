using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class PaymentMethodService : IPaymentMethodService
  {
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public PaymentMethodService(IPaymentMethodRepository paymentMethodRepository)
    {
      _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
    {
      return await _paymentMethodRepository.GetAllAsync();
    }

    public async Task<PaymentMethod> GetPaymentMethodByIdAsync(string id)
    {
      // Validate if the payment method exists
      var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id);
      if (paymentMethod == null)
      {
        throw new KeyNotFoundException($"PaymentMethod with ID '{id}' was not found.");
      }
      return paymentMethod;
    }

    public async Task<PaymentMethod> CreatePaymentMethodAsync(PaymentMethod paymentMethod)
    {
      return await _paymentMethodRepository.CreateAsync(paymentMethod);
    }

    public async Task<bool> UpdatePaymentMethodAsync(string id, PaymentMethod paymentMethod)
    {
      paymentMethod.DateModified = DateTime.UtcNow;
      return await _paymentMethodRepository.UpdateAsync(id, paymentMethod);
    }

    public async Task<bool> DeletePaymentMethodAsync(string id)
    {
      // Validate if the payment method exists
      var paymentMethod = await GetPaymentMethodByIdAsync(id);
      if (paymentMethod == null)
      {
        throw new KeyNotFoundException($"PaymentMethod with ID '{id}' was not found.");
      }
      return await _paymentMethodRepository.DeleteAsync(id);
    }
  }
}
