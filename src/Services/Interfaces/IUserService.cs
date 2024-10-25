using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IUserService
  {
    Task<IEnumerable<UserEmailDto>> GetAllUsersAsync();
    Task<UserEmailDto?> GetUserByIdAsync(string id);
    Task<User> RegisterUserAsync(UserDto userDto);
    Task<User?> LoginUserAsync(UserDto loginDto);
    Task<bool> UpdateUserAsync(string id, UserEmailDto userDto);
    Task<bool> DeleteUserAsync(string id);
  }
}
