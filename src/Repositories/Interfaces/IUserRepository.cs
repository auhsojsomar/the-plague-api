using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IUserRepository
  {
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(string id);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User> CreateUserAsync(User user);
    Task<bool> UpdateUserAsync(string id, User user);
    Task<bool> DeleteUserAsync(string id);
  }
}
