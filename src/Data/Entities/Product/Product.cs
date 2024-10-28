using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using The_Plague_Api.Data.Interface;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace The_Plague_Api.Data.Entities.Product
{
  public class Product : IMongo
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; } = ObjectId.GenerateNewId().ToString();

    [Required, MaxLength(100)]
    public required string Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    public Image? Image { get; set; }

    [Required, Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public Discount? Discount { get; set; } // Optional relationship with a discount

    [Required]
    public ICollection<Variant> Variants { get; set; } = new List<Variant>(); // Ensure initialization
  }
}
