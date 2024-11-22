using System.ComponentModel.DataAnnotations;

namespace The_Plague_Api.Data.Dto
{
  public class UserLoginDto
  {
    public required string Email { get; set; }

    public required string Password { get; set; }
  }
}
