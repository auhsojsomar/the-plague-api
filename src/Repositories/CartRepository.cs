using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Repositories
{
  public class CartRepository : ICartRepository
  {
    private readonly IMongoDbService<Cart> _cartService;

    public CartRepository(IMongoDatabase database)
    {
      const string cartCollection = "carts";
      _cartService = new MongoDbService<Cart>(database, cartCollection);
    }

    public async Task<Cart?> GetByIdAsync(string id)
    {
      return await _cartService.GetAsync(id);
    }

    public async Task<Cart?> GetByUserIdAsync(string userId)
    {
      var filter = Builders<Cart>.Filter.Eq(cart => cart.UserId, userId);
      return await _cartService.GetAsync(filter);
    }

    public async Task<IEnumerable<Cart>> GetAllAsync()
    {
      return await _cartService.GetAllAsync();
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
      return await _cartService.CreateAsync(cart);
    }

    public async Task<bool> UpdateAsync(string id, Cart cart)
    {
      cart.DateModified = DateTime.UtcNow;
      return await _cartService.UpdateAsync(id, cart);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      return await _cartService.DeleteAsync(id);
    }
  }
}
