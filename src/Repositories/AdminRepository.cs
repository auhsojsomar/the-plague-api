using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using MongoDB.Driver;
using The_Plague_Api.Data.Entities.User;
using The_Plague_Api.Services;

namespace The_Plague_Api.Repositories
{
  public class AdminRepository : IAdminRepository
  {
    private readonly IMongoDbService<Admin> _adminService;

    public AdminRepository(IMongoDatabase database)
    {
      const string adminCollection = "admins";
      _adminService = new MongoDbService<Admin>(database, adminCollection);
    }

    public async Task<IEnumerable<Admin>> GetAllAsync()
    {
      return await _adminService.GetAllAsync();
    }

    public async Task<Admin?> GetByIdAsync(string id)
    {
      return await _adminService.GetAsync(id);
    }

    public async Task<Admin?> GetByUsernameAsync(string username)
    {
      var filter = Builders<Admin>.Filter.Eq(a => a.Username, username);
      var admins = await _adminService.GetAllAsync(filter);
      return admins.FirstOrDefault();
    }

    public async Task<Admin> CreateAsync(Admin admin)
    {
      return await _adminService.CreateAsync(admin);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      return await _adminService.DeleteAsync(id);
    }
  }
}
