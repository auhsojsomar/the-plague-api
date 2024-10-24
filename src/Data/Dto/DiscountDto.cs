using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Data.Dto
{
  public class DiscountDto
  {
    public string? Id { get; set; }
    public DiscountType Type { get; set; }
    public decimal Value { get; set; }
  }
}