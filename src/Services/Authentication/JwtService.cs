using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using The_Plague_Api.Settings;

namespace The_Plague_Api.Services.Authentication
{
  public class JwtService
  {
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> jwtSettings)
    {
      _jwtSettings = jwtSettings.Value;
    }

    public (string Token, DateTime Expiration) GenerateJwtToken(string userId, string email)
    {
      var claims = new[]
      {
        new Claim(JwtRegisteredClaimNames.Sub, userId),
        new Claim(JwtRegisteredClaimNames.Email, email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var expiration = DateTime.UtcNow.AddYears(_jwtSettings.ExpiryMinutes);

      var token = new JwtSecurityToken(
          _jwtSettings.Issuer,
          _jwtSettings.Audience,
          claims,
          expires: expiration,
          signingCredentials: creds
      );

      var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

      return (tokenString, expiration);
    }
  }
}
