using The_Plague_Api.Data.Entities.User;

namespace The_Plague_Api.Data.Dto
{
  public class UserDto
  {
    public string? Id { get; set; }

    public required string Email { get; set; }

    public required string FullName { get; set; }

    public required ICollection<ShippingAddress> ShippingAddress { get; set; }
  }
}
