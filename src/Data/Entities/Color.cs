using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace The_Plague_Api.Data.Entities
{
  public class Color
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [Required, MaxLength(50)]
    public required string Name { get; set; }

    [Required, MaxLength(7)]
    public required string HexCode { get; set; }
  }
}