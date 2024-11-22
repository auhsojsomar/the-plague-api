using The_Plague_Api.Data.Entities.User;
using The_Plague_Api.Services.Interfaces;
using MongoDB.Driver;
using The_Plague_Api.Services;
using The_Plague_Api.Repositories.Interfaces;

namespace The_Plague_Api.Repositories
{
  public class UserRepository : IUserRepository
  {
    private readonly IMongoDbService<User> _userService;

    public UserRepository(IMongoDatabase database)
    {
      const string userCollection = "users";
      _userService = new MongoDbService<User>(database, userCollection);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
      return await _userService.GetAllAsync();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
      return await _userService.GetAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
      var filter = Builders<User>.Filter.Eq(u => u.Email, email);
      var users = await _userService.GetAllAsync(filter);

      return users.FirstOrDefault();
    }

    public async Task<User> CreateAsync(User user)
    {
      return await _userService.CreateAsync(user);
    }

    public async Task<bool> UpdateAsync(string id, User user)
    {
      return await _userService.UpdateAsync(id, user);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      return await _userService.DeleteAsync(id);
    }
  }
}
