using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using The_Plague_Api.Data.Interface;

namespace The_Plague_Api.Data.Entities
{
  public class User : IMongo
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public required string Email { get; set; }

    public required string Password { get; set; }
  }
}
