using AutoMapper;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.User;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class UserService : IUserService
  {
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
      _userRepository = userRepository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
      var users = await _userRepository.GetAllAsync();
      return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto?> GetUserByIdAsync(string id)
    {
      var user = await _userRepository.GetByIdAsync(id);
      return _mapper.Map<UserDto>(user);
    }

    public async Task<User> RegisterUserAsync(User user)
    {
      try
      {
        var existingUser = await _userRepository.GetByEmailAsync(user.Email);
        if (existingUser != null)
          throw new ApplicationException("User with this email already exists.");

        // Hash the password
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        // Create the user in the repository
        return await _userRepository.CreateAsync(user);
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Error registering user: {ex.Message}", ex);
      }
    }

    public async Task<User?> LoginUserAsync(UserLoginDto userLoginDto)
    {
      try
      {
        var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);
        if (user == null)
          throw new ApplicationException("Invalid email or password.");

        // Verify the password
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password);
        if (!isPasswordValid)
          throw new ApplicationException("Invalid email or password.");

        return user;
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Error during login: {ex.Message}", ex);
      }
    }

    public async Task<bool> UpdateUserAsync(string id, UserDto userDto)
    {
      var user = await _userRepository.GetByIdAsync(id);
      if (user == null)
        throw new ApplicationException("User not found.");

      user.Email = userDto.Email;

      return await _userRepository.UpdateAsync(id, user);
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
      return await _userRepository.DeleteAsync(id);
    }
  }
}
