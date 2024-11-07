namespace The_Plague_Api.Helpers
{
  public class ValidationHelpers
  {
    public static void ValidateId(string id)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException("ID cannot be null or empty.", nameof(id));
    }
  }
}