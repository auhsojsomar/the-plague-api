using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using The_Plague_Api.Data.Entities.User;
using The_Plague_Api.Data.Interface;

namespace The_Plague_Api.Data.Entities.Order
{
  public class Order : IMongo
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string? UserId { get; set; }

    public string? CartId { get; set; }

    public int OrderStatusKey { get; set; } = 1;

    public int PaymentMethodKey { get; set; } = 1;

    public int PaymentStatusKey { get; set; } = 1;

    [Required]
    public required string PaymentTransactionFile { get; set; }

    [Required]
    public required ShippingAddress ShippingAddress { get; set; }

    public decimal? ShippingFee { get; set; } = 100;

    [Required]
    public required List<OrderItem> Items { get; set; } = new List<OrderItem>();

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;
  }
}