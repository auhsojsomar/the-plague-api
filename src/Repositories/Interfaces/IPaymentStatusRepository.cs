using The_Plague_Api.Data.Entities.Order;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IPaymentStatusRepository
  {
    Task<IEnumerable<PaymentStatus>> GetAllAsync();
    Task<PaymentStatus?> GetByIdAsync(string id);
    Task<PaymentStatus> CreateAsync(PaymentStatus paymentStatus);
    Task<bool> UpdateAsync(string id, PaymentStatus paymentStatus);
    Task<bool> DeleteAsync(string id);
  }
}