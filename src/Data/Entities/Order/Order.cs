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

    [Required]
    public required string UserId { get; set; }

    [Required]
    public required string CartId { get; set; }

    [Required]
    public required string StatusId { get; set; }

    [Required]
    public required string PaymentMethodId { get; set; }

    [Required]
    public required string PaymentStatusId { get; set; }

    [Required]
    public required ShippingAddress ShippingAddress { get; set; }

    public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;
  }
}