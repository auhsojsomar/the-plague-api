using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using The_Plague_Api.Data.Interface;

namespace The_Plague_Api.Data.Entities.Order
{
  public class PaymentMethod : IMongo
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public int Key { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string ImageUrl { get; set; }

    public string? Label { get; set; }

    public int IsActive { get; set; } = 1;

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;
  }
}