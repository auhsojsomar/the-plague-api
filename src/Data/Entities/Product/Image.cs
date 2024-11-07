namespace The_Plague_Api.Data.Entities.Product
{
  public class Image
  {
    public string? Main { get; set; }
    public List<string>? Thumbnails { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.UtcNow;

    public DateTime DateModified { get; set; } = DateTime.UtcNow;
  }
}