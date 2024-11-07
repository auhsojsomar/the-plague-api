using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using The_Plague_Api.Data.Interface;

namespace The_Plague_Api.Data.Entities.Product
{
  public class Color : IMongo
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [Required, MaxLength(50)]
    public required string Name { get; set; }

    [Required, MaxLength(7)]
    public required string HexCode { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;
  }
}