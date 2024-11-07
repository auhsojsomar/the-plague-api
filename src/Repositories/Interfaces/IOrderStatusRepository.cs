using The_Plague_Api.Data.Entities.Order;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IOrderStatusRepository
  {
    Task<OrderStatus> CreateAsync(OrderStatus orderStatus);
    Task<IEnumerable<OrderStatus>> GetAllAsync();
    Task<OrderStatus?> GetByIdAsync(string id);
    Task<bool> UpdateAsync(string id, OrderStatus orderStatus);
    Task<bool> DeleteAsync(string id);
  }
}
