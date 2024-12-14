using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.Product;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IProductRepository
  {
    Task<IEnumerable<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(string id);
    Task<Product?> GetByNameAsync(string name);
    Task<Product> CreateAsync(Product product);
    Task<bool> UpdateAsync(string id, Product product);
    Task<bool> DeleteAsync(string id);
    Task<IEnumerable<SizeDto>> GetUniqueSizesAsync();
    Task<IEnumerable<ColorDto>> GetUniqueColorsAsync();
    Task<Color?> GetColorByNameAsync(string name);
    Task<Size?> GetSizeByNameAsync(string name);
    Task<Color> CreateColorAsync(Color color);
    Task<Size> CreateSizeAsync(Size size);
  }
}
