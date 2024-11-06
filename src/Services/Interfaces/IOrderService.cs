using The_Plague_Api.Data.Entities.Order;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IOrderService
  {
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<Order?> GetOrderByIdAsync(string id);
    Task<Order> CreateOrderAsync(Order order);
    Task<bool> UpdateOrderAsync(string id, Order order);
    Task<bool> DeleteOrderAsync(string id);
  }
}