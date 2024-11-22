using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.User;
using The_Plague_Api.Services.Authentication;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  // UserController handles operations related to user management (e.g., registration, login, update, and delete).
  [ApiController]
  [Route("api/[controller]")]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService; // Service to handle user-related operations.
    private readonly JwtService _jwtService;    // Service to handle JWT token generation.

    // Constructor: Injects the IUserService and JwtService dependencies.
    public UserController(IUserService userService, JwtService jwtService)
    {
      _userService = userService;
      _jwtService = jwtService;
    }

    // GET: api/user
    // Retrieves all registered users.
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      var users = await _userService.GetAllUsersAsync();
      return Ok(users); // Returns 200 OK with the list of users.
    }

    // GET: api/user/{id}
    // Retrieves a specific user by their ID.
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
      var user = await _userService.GetUserByIdAsync(id);
      return user is not null
          ? Ok(user) // Returns 200 OK with the user data if found.
          : NotFound(new { Message = "User not found" }); // Returns 404 if the user is not found.
    }

    // POST: api/user/register
    // Registers a new user in the system.
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] User user)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState); // Returns 400 if the request data is invalid.

      try
      {
        var newUser = await _userService.RegisterUserAsync(user);

        if (newUser?.Id == null)
          return StatusCode(500, new { Error = "User ID is null after registration." }); // Returns 500 if no ID is generated.

        // Returns 201 Created with the newly registered user's ID.
        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = newUser.Id },
            new { Message = "User registered successfully", UserId = newUser.Id }
        );
      }
      catch (ApplicationException ex)
      {
        return Conflict(new { Error = ex.Message }); // Returns 409 Conflict for application-specific exceptions.
      }
    }

    // POST: api/user/login
    // Logs in an existing user and generates a JWT token.
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto userLoginDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState); // Returns 400 for invalid input.

      try
      {
        var user = await _userService.LoginUserAsync(userLoginDto);
        if (user == null || user.Id == null)
          return Unauthorized(new { Message = "Invalid email or password" }); // Returns 401 for incorrect credentials.

        // Generates a JWT token for the logged-in user.
        var (token, expiration) = _jwtService.GenerateJwtToken(user.Id, user.Email);

        // Returns 200 OK with the token, expiration time, and user ID.
        return Ok(new
        {
          Token = token,
          ExpiresAt = expiration,
          UserId = user.Id
        });
      }
      catch (ApplicationException ex)
      {
        return Unauthorized(new { Error = ex.Message }); // Returns 401 for known exceptions during login.
      }
      catch (Exception ex)
      {
        return StatusCode(500, new { Error = ex.Message }); // Catches unexpected errors and returns 500.
      }
    }

    // PUT: api/user/{id}
    // Updates a user's email or other details.
    [Authorize] // Requires authorization to update user information.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] UserDto userDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState); // Returns 400 for invalid input.

      try
      {
        var updated = await _userService.UpdateUserAsync(id, userDto);
        return updated
            ? Ok(new { Message = "User updated successfully" }) // Returns 200 if the update is successful.
            : NotFound(new { Message = "User not found" }); // Returns 404 if the user is not found.
      }
      catch (ApplicationException ex)
      {
        return Conflict(new { Error = ex.Message }); // Returns 409 Conflict for specific exceptions.
      }
    }

    // DELETE: api/user/{id}
    // Deletes a user by their ID.
    [Authorize] // Requires authorization to delete a user.
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
      var deleted = await _userService.DeleteUserAsync(id);
      return deleted
          ? Ok(new { Message = "User deleted successfully" }) // Returns 200 if the user is successfully deleted.
          : NotFound(new { Message = "User not found" }); // Returns 404 if the user is not found.
    }
  }
}
