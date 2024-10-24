using MongoDB.Bson;
using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IProductRepository
  {
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(string id);
    Task<Product> CreateAsync(Product product);
    Task<bool> UpdateAsync(string id, Product product);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<string>> GetUniqueSizesAsync();
    Task<IEnumerable<Color>> GetUniqueColorsAsync();
  }
}
