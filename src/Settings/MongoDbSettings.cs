namespace The_Plague_Api.Settings
{
  public class MongoDbSettings
  {
    public required string Host { get; set; }
    public required string Port { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string DatabaseName { get; set; }
    public string ConnectionString => $"mongodb+srv://{Username}:{Password}@{Host}:{Port}";
  }
}