using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.Order;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IOrderRepository
  {
    Task<IEnumerable<Order>> GetAllAsync();

    Task<Order?> GetByIdAsync(string id);

    Task<Order> CreateAsync(Order order);

    Task<bool> UpdateAsync(string id, Order order);

    Task<bool> DeleteAsync(string id);
  }
}
