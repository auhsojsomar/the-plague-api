using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using The_Plague_Api.Data.Interface;

namespace The_Plague_Api.Data.Entities.Order
{
  public class Cart : IMongo
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [Required]
    public required string UserId { get; set; }

    [Required]
    public required List<CartItem> Items { get; set; } = new List<CartItem>();

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;
  }
}