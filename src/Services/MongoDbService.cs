using The_Plague_Api.Services.Interfaces;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using The_Plague_Api.Data.Interface;

namespace The_Plague_Api.Services
{
  public class MongoDbService<T> : IMongoDbService<T> where T : IMongo
  {
    private readonly IMongoCollection<T> _collection;

    public MongoDbService(IMongoDatabase database, string collectionName)
    {
      _collection = database.GetCollection<T>(collectionName);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
      try
      {
        return await _collection.Find(Builders<T>.Filter.Empty).ToListAsync();
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Failed to retrieve all data: {ex.Message}", ex);
      }
    }

    public async Task<IEnumerable<T>> GetAllAsync(FilterDefinition<T> filter)
    {
      try
      {
        return await _collection.Find(filter).ToListAsync();
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Failed to retrieve filtered data: {ex.Message}", ex);
      }
    }

    public async Task<T?> GetAsync(string id)
    {
      try
      {
        var filter = Builders<T>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Failed to retrieve entity with ID {id}: {ex.Message}", ex);
      }
    }

    public async Task<T?> GetAsync(FilterDefinition<T> filter)
    {
      try
      {
        return await _collection.Find(filter).FirstOrDefaultAsync(); ;
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Failed to retrieve filtered data: {ex.Message}", ex);
      }
    }

    public async Task<T> CreateAsync(T entity)
    {
      try
      {
        await _collection.InsertOneAsync(entity);
        return entity;
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Failed to create entity: {ex.Message}", ex);
      }
    }

    public async Task<bool> UpdateAsync(string id, T updatedEntity)
    {
      try
      {
        var filter = Builders<T>.Filter.Eq("Id", id);
        var result = await _collection.ReplaceOneAsync(filter, updatedEntity);

        return result.IsAcknowledged && result.ModifiedCount > 0;
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Failed to update entity with ID {id}: {ex.Message}", ex);
      }
    }

    public async Task<bool> DeleteAsync(string id)
    {
      try
      {
        var filter = Builders<T>.Filter.Eq("Id", id);
        var result = await _collection.DeleteOneAsync(filter);

        return result.IsAcknowledged && result.DeletedCount > 0;
      }
      catch (Exception ex)
      {
        throw new ApplicationException($"Failed to delete entity with ID {id}: {ex.Message}", ex);
      }
    }

  }
}
