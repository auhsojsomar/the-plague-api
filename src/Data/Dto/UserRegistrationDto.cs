using System.ComponentModel.DataAnnotations;

namespace The_Plague_Api.Data.Dto
{
  public class UserRegistrationDto
  {
    [Required(ErrorMessage = "First name is required.")]
    public required string FirstName { get; set; }

    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Last name is required.")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
    public required string Password { get; set; }
  }
}