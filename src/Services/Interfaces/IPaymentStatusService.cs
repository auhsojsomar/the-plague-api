using The_Plague_Api.Data.Entities.Order;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IPaymentStatusService
  {
    Task<IEnumerable<PaymentStatus>> GetAllPaymentStatusesAsync();
    Task<PaymentStatus?> GetPaymentStatusByIdAsync(string id);
    Task<PaymentStatus> CreatePaymentStatusAsync(PaymentStatus paymentStatus);
    Task<bool> UpdatePaymentStatusAsync(string id, PaymentStatus paymentStatus);
    Task<bool> DeletePaymentStatusAsync(string id);
  }
}