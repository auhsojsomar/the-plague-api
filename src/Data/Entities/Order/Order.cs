using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
    public required string ProductId { get; set; }

    [Required]
    public required string VariantId { get; set; }

    [Required]
    public required string StatusId { get; set; }

    [Required]
    public required string PaymentMethodId { get; set; }

    [Required]
    public required int Quantity { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;
  }
}