using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Services.Authentication;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly JwtService _jwtService;

    public UserController(IUserService userService, JwtService jwtService)
    {
      _userService = userService;
      _jwtService = jwtService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
      var users = await _userService.GetAllUsersAsync();
      return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(string id)
    {
      var user = await _userService.GetUserByIdAsync(id);
      return user is not null
          ? Ok(user)
          : NotFound(new { Message = "User not found" });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserDto userDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        var newUser = await _userService.RegisterUserAsync(userDto);
        return CreatedAtAction(nameof(GetUserByIdAsync), new { id = newUser.Id },
            new { Message = "User registered successfully", UserId = newUser.Id });
      }
      catch (ApplicationException ex)
      {
        return Conflict(new { Error = ex.Message });
      }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserDto loginDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        var user = await _userService.LoginUserAsync(loginDto);

        // Ensure user is not null before accessing properties
        if (user == null)
        {
          return Unauthorized(new { Message = "Invalid email or password" });
        }

        // At this point, we can safely access user.Id and user.Email
        if (user.Id == null)
        {
          return StatusCode(500, new { Error = "User ID is null" });
        }

        // Generate JWT token
        var (token, expiration) = _jwtService.GenerateJwtToken(user.Id, user.Email);

        // Return token, expiration, and user info
        return Ok(new
        {
          Token = token,
          ExpiresAt = expiration, // More precise expiration info
          UserId = user.Id
        });
      }
      catch (ApplicationException ex)
      {
        // Handle known exceptions (e.g., login-related issues)
        return Unauthorized(new { Error = ex.Message });
      }
      catch (Exception ex)
      {
        // Catch unexpected exceptions to avoid exposing sensitive info
        return StatusCode(500, new { Error = ex.Message });
      }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] UserEmailDto userDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        var updated = await _userService.UpdateUserAsync(id, userDto);
        return updated
            ? Ok(new { Message = "User updated successfully" })
            : NotFound(new { Message = "User not found" });
      }
      catch (ApplicationException ex)
      {
        return Conflict(new { Error = ex.Message });
      }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserAsync(string id)
    {
      var deleted = await _userService.DeleteUserAsync(id);
      return deleted
          ? Ok(new { Message = "User deleted successfully" })
          : NotFound(new { Message = "User not found" });
    }
  }
}
