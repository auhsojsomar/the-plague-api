using System.Text.Json.Serialization;
using The_Plague_Api.Data.Entities.Product;

namespace The_Plague_Api.Data.Dto
{
  public class VariantDto
  {
    public string? Id { get; set; }
    public SizeDto Size { get; set; } = new SizeDto();
    public ColorDto Color { get; set; } = new ColorDto();
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // Ignore when null
    public DiscountDto? Discount { get; set; }

    // Read-only property to calculate SalePrice
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] // Ignore when null
    public decimal? SalePrice => GetSalePrice();

    // Method to get SalePrice only if Discount exists
    public decimal? GetSalePrice()
    {
      if (Discount == null) return null;

      return Discount.Type switch
      {
        DiscountType.Percentage => Price - (Price * (Discount.Value / 100)),
        DiscountType.FixedAmount => Price - Discount.Value,
        _ => Price // Fallback to original Price if type is unknown
      };
    }
  }
}