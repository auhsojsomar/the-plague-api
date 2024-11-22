namespace The_Plague_Api.Data.Dto
{
  public class BaseDto
  {
    public int Key { get; set; }
    public string Name { get; set; } = string.Empty;
    public int IsActive { get; set; }
  }
}