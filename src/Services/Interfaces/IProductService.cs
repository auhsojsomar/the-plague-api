using MongoDB.Bson;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IProductService
  {
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetProductByIdAsync(string id);
    Task<ProductDto> CreateProductAsync(ProductDto product);
    Task<bool> UpdateProductAsync(string id, ProductDto product);
    Task<bool> DeleteProductAsync(string id);
    Task<IEnumerable<string>> GetUniqueSizesAsync();
    Task<IEnumerable<ColorDto>> GetUniqueColorsAsync();
  }
}
