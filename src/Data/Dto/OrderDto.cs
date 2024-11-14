using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Data.Entities.User;

namespace The_Plague_Api.Data.Dto
{
  public class OrderDto
  {
    public string? Id { get; set; }
    public UserDto? User { get; set; }

    // Using BaseDto for each status-related property
    public required BaseDto OrderStatus { get; set; }

    public required BaseDto PaymentMethod { get; set; }

    public required BaseDto PaymentStatus { get; set; }

    public required BaseDto ShippingFee { get; set; }

    public required string PaymentTransactionFile { get; set; }

    public ShippingAddress? ShippingAddress { get; set; }

    public decimal? TotalPrice { get; set; }

    public required List<OrderItemDto> Items { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateModified { get; set; }

  }
}
