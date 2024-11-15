
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Data.Entities.User;

namespace The_Plague_Api.Data.Dto
{
  public class UpdateOrderDto
  {
    public int? OrderStatusKey { get; set; }
    public int? PaymentMethodKey { get; set; }
    public int? PaymentStatusKey { get; set; }
    public int? ShippingFeeKey { get; set; }
    public decimal? TotalPrice { get; set; }
    public List<OrderItem>? Items { get; set; }
    public ShippingAddress? ShippingAddress { get; set; }
    public string? PaymentTransactionFile { get; set; }
  }

}