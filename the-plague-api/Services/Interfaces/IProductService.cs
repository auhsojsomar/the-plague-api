using MongoDB.Bson;
using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IProductService
  {
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task<Product?> GetProductByIdAsync(string id);
    Task<Product> CreateProductAsync(Product product);
    Task<bool> UpdateProductAsync(string id, Product product);
    Task<bool> DeleteProductAsync(string id);
    Task<IEnumerable<string>> GetUniqueSizesAsync();
    Task<IEnumerable<Color>> GetUniqueColorsAsync();
  }
}
