using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace The_Plague_Api.Services
{
  public class KeyGeneratorService
  {
    private readonly IMongoCollection<BsonDocument> _counterCollection;

    public KeyGeneratorService(IMongoDatabase database)
    {
      _counterCollection = database.GetCollection<BsonDocument>("counter");
    }

    public async Task<int> GenerateUniqueKeyAsync<T>(string counterId, IMongoCollection<T> collection, Expression<Func<T, int>> keyField)
    {
      var filter = Builders<BsonDocument>.Filter.Eq("_id", counterId);
      var update = Builders<BsonDocument>.Update.Inc("sequence_value", 1);
      var options = new FindOneAndUpdateOptions<BsonDocument>
      {
        IsUpsert = true,
        ReturnDocument = ReturnDocument.After
      };

      var result = await _counterCollection.FindOneAndUpdateAsync(filter, update, options);
      int nextKey = result["sequence_value"].AsInt32;

      // Check for uniqueness in the provided collection
      var keyFilter = Builders<T>.Filter.Eq(keyField, nextKey);
      var existingRecord = await collection.Find(keyFilter).FirstOrDefaultAsync();

      if (existingRecord != null)
      {
        throw new InvalidOperationException($"The generated key {nextKey} already exists.");
      }

      return nextKey;
    }
  }
}
