using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace The_Plague_Api.Data.Entities.User
{
  public class Address
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public int Default { get; set; }

    [Required]
    public required string FullName { get; set; }

    [Required]
    public required string FullAddress { get; set; }

    [Required]
    public required string ContactNumber { get; set; }
  }
}