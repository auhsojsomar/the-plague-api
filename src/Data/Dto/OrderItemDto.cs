namespace The_Plague_Api.Data.Dto
{
  public class OrderItemDto
  {
    public required string Product { get; set; }

    public required string Variant { get; set; }

    public int Quantity { get; set; }
  }
}