using The_Plague_Api.Data.Entities.Order;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IPaymentMethodService
  {
    Task<IEnumerable<PaymentMethod>> GetAllPaymentMethodsAsync();
    Task<PaymentMethod> GetPaymentMethodByIdAsync(string id);
    Task<PaymentMethod> CreatePaymentMethodAsync(PaymentMethod paymentMethod);
    Task<bool> UpdatePaymentMethodAsync(string id, PaymentMethod paymentMethod);
    Task<bool> DeletePaymentMethodAsync(string id);
  }
}
