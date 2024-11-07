namespace The_Plague_Api.Data.Entities.Order
{
  public class OrderItem
  {
    public required string ProductId { get; set; }

    public required string VariantId { get; set; }

    public int Quantity { get; set; }

  }
}