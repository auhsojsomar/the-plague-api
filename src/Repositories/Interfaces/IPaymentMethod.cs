using The_Plague_Api.Data.Entities.Order;

public interface IPaymentMethodRepository
{
  Task<IEnumerable<PaymentMethod>> GetAllAsync();
  Task<PaymentMethod?> GetByIdAsync(string id);
  Task<PaymentMethod?> GetByNameAsync(string name);
  Task<PaymentMethod> CreateAsync(PaymentMethod paymentMethod);
  Task<bool> UpdateAsync(string id, PaymentMethod paymentMethod);
  Task<bool> DeleteAsync(string id);
}
