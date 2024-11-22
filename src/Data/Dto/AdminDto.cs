using System.ComponentModel.DataAnnotations;

namespace The_Plague_Api.Data.Dto
{
  public class AdminDto
  {
    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }
  }
}
