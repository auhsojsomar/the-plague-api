using System.Drawing;

namespace The_Plague_Api.Data.Dto
{
  public class VariantDto
  {
    public SizeDto Size { get; set; } = new SizeDto();
    public ColorDto Color { get; set; } = new ColorDto();
    public decimal Price { get; set; }
    public int Quantity { get; set; }
  }
}