using AutoMapper;
using The_Plague_Api.Data.Entities.User;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Data.Dto;
using System.Security.Authentication;

namespace The_Plague_Api.Services
{
  public class AdminService : IAdminService
  {
    private readonly IAdminRepository _adminRepository;
    private readonly IMapper _mapper;

    public AdminService(IAdminRepository adminRepository, IMapper mapper)
    {
      _adminRepository = adminRepository;
      _mapper = mapper;
    }

    // Get all admins
    public async Task<IEnumerable<AdminDto>> GetAllAdminsAsync()
    {
      var admins = await _adminRepository.GetAllAsync();
      return _mapper.Map<IEnumerable<AdminDto>>(admins);
    }

    // Get admin by ID
    public async Task<AdminDto?> GetAdminByIdAsync(string id)
    {
      var admin = await _adminRepository.GetByIdAsync(id);
      return admin == null ? null : _mapper.Map<AdminDto>(admin);
    }

    // Register new admin
    public async Task<Admin> RegisterAdminAsync(Admin admin)
    {
      try
      {
        var existingAdmin = await _adminRepository.GetByUsernameAsync(admin.Username);
        if (existingAdmin != null)
          throw new ApplicationException("Admin with this username already exists.");

        // Hash the password
        admin.Password = BCrypt.Net.BCrypt.HashPassword(admin.Password);

        // Create the admin in the repository
        return await _adminRepository.CreateAsync(admin);
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Error registering admin: {ex.Message}", ex);
      }
    }

    // Login admin and return JWT token
    public async Task<Admin?> LoginAdminAsync(AdminDto adminDto)
    {
      try
      {
        var admin = await _adminRepository.GetByUsernameAsync(adminDto.Username);
        if (admin == null || !VerifyPassword(adminDto.Password, admin.Password))
        {
          throw new AuthenticationException("Invalid username or password.");
        }

        return admin;
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Error during login: {ex.Message}", ex);
      }
    }

    // Delete admin by ID
    public async Task<bool> DeleteAdminAsync(string id)
    {
      return await _adminRepository.DeleteAsync(id);
    }

    private bool VerifyPassword(string plainPassword, string hashedPassword)
    {
      return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
  }
}
