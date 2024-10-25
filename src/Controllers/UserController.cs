using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserDto userDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        var newUser = await _userService.RegisterUserAsync(userDto);
        return CreatedAtAction(nameof(GetUserByIdAsync), new { id = newUser.Id }, new { Message = "User registered successfully", UserId = newUser.Id });
      }
      catch (ApplicationException ex)
      {
        return Conflict(new { Error = ex.Message });
      }
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserDto loginDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      try
      {
        var user = await _userService.LoginUserAsync(loginDto);
        return user is not null
            ? Ok(new { Message = "Login successful", UserId = user.Id })
            : NotFound(new { Message = "User not found" });
      }
      catch (ApplicationException ex)
      {
        return Unauthorized(new { Error = ex.Message });
      }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByIdAsync(string id)
    {
      var user = await _userService.GetUserByIdAsync(id);
      return user is not null
          ? Ok(user)
          : NotFound(new { Message = "User not found" });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsersAsync()
    {
      var users = await _userService.GetAllUsersAsync();
      return Ok(users);
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
