using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class OrderStatusService : IOrderStatusService
  {
    private readonly IOrderStatusRepository _statusRepository;

    public OrderStatusService(IOrderStatusRepository statusRepository)
    {
      _statusRepository = statusRepository;
    }

    public async Task<OrderStatus> CreateAsync(OrderStatus status)
    {
      return await _statusRepository.CreateAsync(status);
    }

    public async Task<IEnumerable<OrderStatus>> GetAllAsync()
    {
      return await _statusRepository.GetAllAsync();
    }

    public async Task<OrderStatus?> GetByIdAsync(string id)
    {
      return await _statusRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(string id, OrderStatus status)
    {
      status.DateModified = DateTime.UtcNow;
      return await _statusRepository.UpdateAsync(id, status);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      return await _statusRepository.DeleteAsync(id);
    }
  }
}
