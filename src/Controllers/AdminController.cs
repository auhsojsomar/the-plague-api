using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.User;
using The_Plague_Api.Services.Authentication;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class AdminController : ControllerBase
  {
    private readonly IAdminService _adminService;
    private readonly JwtService _jwtService;

    public AdminController(IAdminService adminService, JwtService jwtService)
    {
      _adminService = adminService;
      _jwtService = jwtService;
    }

    // GET: api/admin
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      var admins = await _adminService.GetAllAdminsAsync();
      return Ok(admins);
    }

    // GET: api/admin/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
      var admin = await _adminService.GetAdminByIdAsync(id);
      return admin is not null
          ? Ok(admin)
          : NotFound(new { Message = "Admin not found" });
    }

    // POST: api/admin/register
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] Admin admin)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        var newAdmin = await _adminService.RegisterAdminAsync(admin);

        if (newAdmin?.Id == null)
          return StatusCode(500, new { Error = "Admin ID is null after registration." });

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = newAdmin.Id },
            new { Message = "Admin registered successfully", AdminId = newAdmin.Id }
        );
      }
      catch (ApplicationException ex)
      {
        return Conflict(new { Error = ex.Message });
      }
    }

    // POST: api/admin/login
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] AdminDto adminLoginDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        var admin = await _adminService.LoginAdminAsync(adminLoginDto);
        if (admin == null || admin.Id == null)
          return Unauthorized(new { Message = "Invalid email or password" });

        // Generate JWT token for the admin
        var (token, expiration) = _jwtService.GenerateJwtToken(admin.Id, admin.Username);

        return Ok(new
        {
          Token = token,
          ExpiresAt = expiration,
          AdminId = admin.Id
        });
      }
      catch (ApplicationException ex)
      {
        return Unauthorized(new { Error = ex.Message });
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { Error = ex.Message });
      }
    }

    // DELETE: api/admin/{id}
    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
      var deleted = await _adminService.DeleteAdminAsync(id);
      return deleted
          ? Ok(new { Message = "Admin deleted successfully" })
          : NotFound(new { Message = "Admin not found" });
    }
  }
}
