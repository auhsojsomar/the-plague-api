namespace The_Plague_Api.Settings
{
  public class MongoDbSettings
  {
    public required string Host { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string DatabaseName { get; set; }
    public string ConnectionString => $"mongodb+srv://{Username}:{Password}@{Host}/?retryWrites=true&w=majority&appName=the-plague-cluster";
  }
}