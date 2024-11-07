using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.User;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IUserService
  {
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(string id);
    Task<User> RegisterUserAsync(User user);
    Task<User?> LoginUserAsync(UserLoginDto userLoginDto);
    Task<bool> UpdateUserAsync(string id, UserDto userDto);
    Task<bool> DeleteUserAsync(string id);
  }
}
