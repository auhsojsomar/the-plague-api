namespace The_Plague_Api.Helpers
{
  public static class StringHelpers
  {
    // Convert string to kebab case
    public static string ToKebabCase(string value)
    {
      if (string.IsNullOrWhiteSpace(value)) return value;

      return string.Concat(
          value.Select((ch, i) =>
              char.IsWhiteSpace(ch)
                  ? '-' // Replace spaces with hyphens
                  : char.ToLower(ch)
          )
      ).Trim('-'); // Remove any leading or trailing hyphens
    }
  }
}
