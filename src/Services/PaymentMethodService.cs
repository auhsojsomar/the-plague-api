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

    public Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync()
    {
      return _paymentMethodRepository.GetAllAsync();
    }

    public Task<PaymentMethod> GetPaymentMethodByIdAsync(string id)
    {
      return _paymentMethodRepository.GetByIdAsync(id);
    }

    public Task<PaymentMethod> CreatePaymentMethodAsync(PaymentMethod paymentMethod)
    {
      return _paymentMethodRepository.CreateAsync(paymentMethod);
    }

    public Task<bool> UpdatePaymentMethodAsync(string id, PaymentMethod paymentMethod)
    {
      return _paymentMethodRepository.UpdateAsync(id, paymentMethod);
    }

    public Task<bool> DeletePaymentMethodAsync(string id)
    {
      return _paymentMethodRepository.DeleteAsync(id);
    }
  }
}
