namespace The_Plague_Api.Data.Dto
{
  public class ProductDto
  {
    public string? Id { get; set; }
    public string Name { get; set; } = String.Empty;
    public string? Description { get; set; }
    public ImageDto? Image { get; set; }
    public List<VariantDto> Variants { get; set; } = new List<VariantDto>();
  }
}