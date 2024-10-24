using MongoDB.Driver;
using The_Plague_Api.Settings;

namespace The_Plague_Api.Extension
{
  public static class MongoDbConnection
  {
    public static IServiceCollection AddMongoDbConnection(this IServiceCollection services)
    {
      // Inject Mongo Client
      services.AddSingleton<IMongoClient>(serviceProvider =>
      {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var mongoSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

        return new MongoClient(mongoSettings?.ConnectionString);
      });

      // Inject MongoDB Connection
      services.AddSingleton(serviceProvider =>
      {
        // Mongo Client
        var client = serviceProvider.GetRequiredService<IMongoClient>();
        // Mongo Settings
        var mongoSettings = serviceProvider.GetRequiredService<IConfiguration>()
                  .GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();

        return client.GetDatabase(mongoSettings?.DatabaseName);
      });

      return services;
    }
  }
}
