using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.User;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IAdminService
  {
    Task<IEnumerable<AdminDto>> GetAllAdminsAsync(); // Get all admins
    Task<AdminDto?> GetAdminByIdAsync(string id); // Get admin by ID
    Task<Admin> RegisterAdminAsync(Admin admin); // Register a new admin
    Task<Admin?> LoginAdminAsync(AdminDto adminDto); // Login admin and return JWT token
    Task<bool> DeleteAdminAsync(string id); // Delete admin by ID
  }
}
