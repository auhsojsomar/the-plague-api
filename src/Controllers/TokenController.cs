using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using The_Plague_Api.Services.Authentication;

namespace The_Plague_Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class TokenController : ControllerBase
  {
    private readonly JwtService _jwtService;
    private readonly ILogger<TokenController> _logger;

    public TokenController(JwtService jwtService, ILogger<TokenController> logger)
    {
      _jwtService = jwtService;
      _logger = logger;
    }

    [HttpPost("verify-token")]
    public IActionResult VerifyToken([FromHeader] string Authorization)
    {
      if (string.IsNullOrEmpty(Authorization))
      {
        return BadRequest(new { message = "Token is required" });
      }

      // Extract the token part after "Bearer "
      var token = Authorization.StartsWith("Bearer ")
          ? Authorization.Substring(7) // Remove "Bearer " prefix
          : null;

      if (string.IsNullOrEmpty(token))
      {
        return BadRequest(new { message = "Invalid token format" });
      }

      // Verify token logic
      var principal = _jwtService.VerifyJwtToken(token);
      if (principal == null)
      {
        return Unauthorized(new { message = "Invalid or expired token." });
      }

      // Log all claims for inspection
      foreach (var claim in principal.Claims)
      {
        _logger.LogInformation("Claim {Type}: {Value}", claim.Type, claim.Value);
      }

      var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var email = principal.FindFirst(ClaimTypes.Email)?.Value;

      return Ok(new { userId, email });
    }

  }
}