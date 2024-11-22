using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using The_Plague_Api.Services.Authentication;
using The_Plague_Api.Settings;

namespace The_Plague_Api.Extensions
{
  public static class JwtExtensions
  {
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
      // Bind JwtSettings from configuration
      services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

      // Register JwtService with DI
      services.AddSingleton<JwtService>();

      // Get key, issuer, and audience from configuration
      var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();

      // Check if jwtSettings is null and throw an exception
      if (jwtSettings == null)
      {
        throw new ArgumentNullException(nameof(jwtSettings), "JwtSettings is not configured in appsettings.json.");
      }

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      })
      .AddJwtBearer(options =>
      {
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = jwtSettings.Issuer,
          ValidAudience = jwtSettings.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
          ClockSkew = TimeSpan.Zero // Exactly expire the token for the given
        };
      });

      return services;
    }
  }
}
