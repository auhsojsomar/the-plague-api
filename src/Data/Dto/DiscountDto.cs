using The_Plague_Api.Data.Entities.Product;

namespace The_Plague_Api.Data.Dto
{
  public class DiscountDto
  {
    public DiscountType Type { get; set; }
    public decimal Value { get; set; }
  }
}