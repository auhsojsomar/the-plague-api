using MongoDB.Driver;
using The_Plague_Api.Data.Interface;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IMongoDbService<T> where T : IMongo
  {
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(FilterDefinition<T> filter);
    Task<T?> GetAsync(string id);
    Task<T?> GetAsync(FilterDefinition<T> filter);
    Task<T> CreateAsync(T entity);
    Task<bool> UpdateAsync(string id, T updatedEntity);
    Task<bool> DeleteAsync(string id);
  }
}
