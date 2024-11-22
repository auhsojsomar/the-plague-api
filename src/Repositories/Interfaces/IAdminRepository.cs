using The_Plague_Api.Data.Entities.User;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IAdminRepository
  {
    Task<IEnumerable<Admin>> GetAllAsync();
    Task<Admin?> GetByIdAsync(string id);
    Task<Admin?> GetByUsernameAsync(string username);
    Task<Admin> CreateAsync(Admin admin);
    Task<bool> DeleteAsync(string id);
  }
}
