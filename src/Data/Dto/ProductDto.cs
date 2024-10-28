using The_Plague_Api.Data.Entities.Product;

namespace The_Plague_Api.Data.Dto
{
  public class ProductDto
  {
    public string? Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string? Description { get; set; }
    public Image? Image { get; set; }
    public decimal Price { get; set; }
    public DiscountDto? Discount { get; set; }
    public List<VariantDto> Variants { get; set; } = new List<VariantDto>();
  }
}