using System.Text.Json.Serialization;
using The_Plague_Api.Data.Entities.Product;
using The_Plague_Api.Helpers;

namespace The_Plague_Api.Data.Dto
{
  public class VariantDto
  {
    public string? Id { get; set; }

    public SizeDto Size { get; set; } = new SizeDto();

    public ColorDto Color { get; set; } = new ColorDto();

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DiscountDto? Discount { get; set; }

    // Read-only property to calculate SalePrice using PriceHelper
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public decimal? SalePrice => PriceHelpers.CalculateSalePrice(Price, Discount?.Type, Discount?.Value);
  }
}
