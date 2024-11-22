using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class PaymentStatusService : IPaymentStatusService
  {
    private readonly IPaymentStatusRepository _paymentStatusRepository;

    public PaymentStatusService(IPaymentStatusRepository paymentStatusRepository)
    {
      _paymentStatusRepository = paymentStatusRepository;
    }

    public async Task<IEnumerable<PaymentStatus>> GetAllPaymentStatusesAsync()
    {
      return await _paymentStatusRepository.GetAllAsync();
    }

    public async Task<PaymentStatus?> GetPaymentStatusByIdAsync(string id)
    {
      return await _paymentStatusRepository.GetByIdAsync(id);
    }

    public async Task<PaymentStatus> CreatePaymentStatusAsync(PaymentStatus paymentStatus)
    {
      return await _paymentStatusRepository.CreateAsync(paymentStatus);
    }

    public async Task<bool> UpdatePaymentStatusAsync(string id, PaymentStatus paymentStatus)
    {
      return await _paymentStatusRepository.UpdateAsync(id, paymentStatus);
    }

    public async Task<bool> DeletePaymentStatusAsync(string id)
    {
      return await _paymentStatusRepository.DeleteAsync(id);
    }
  }
}
