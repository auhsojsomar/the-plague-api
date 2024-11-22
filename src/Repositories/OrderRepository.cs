using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Services;
using static The_Plague_Api.Helpers.ValidationHelpers;

namespace The_Plague_Api.Repositories
{
  public class OrderRepository : IOrderRepository
  {
    private readonly IMongoDbService<Order> _orderService;

    public OrderRepository(IMongoDatabase database)
    {
      const string orderCollection = "orders";
      _orderService = new MongoDbService<Order>(database, orderCollection);
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
      return await _orderService.GetAllAsync();
    }

    public async Task<Order?> GetByIdAsync(string id)
    {
      ValidateId(id);
      return await _orderService.GetAsync(id);
    }

    public async Task<Order> CreateAsync(Order order)
    {
      return await _orderService.CreateAsync(order);
    }

    public async Task<bool> UpdateAsync(string id, Order order)
    {
      ValidateId(id);
      order.Id = id;
      order.DateModified = DateTime.UtcNow;
      return await _orderService.UpdateAsync(id, order);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      ValidateId(id);
      return await _orderService.DeleteAsync(id);
    }
  }
}
