using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Product;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Helpers;
using The_Plague_Api.Services;
using static The_Plague_Api.Helpers.ValidationHelpers;

namespace The_Plague_Api.Repositories
{
  public class ProductRepository : IProductRepository
  {
    private readonly IMongoDbService<Product> _productService;
    private readonly IMongoDbService<Size> _sizeService;
    private readonly IMongoDbService<Color> _colorService;

    public ProductRepository(IMongoDatabase database)
    {
      const string productCollection = "products";
      const string sizeCollection = "sizes";
      const string colorCollection = "colors";

      _productService = new MongoDbService<Product>(database, productCollection);
      _sizeService = new MongoDbService<Size>(database, sizeCollection);
      _colorService = new MongoDbService<Color>(database, colorCollection);
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
      return _productService.GetAllAsync();
    }

    public Task<Product?> GetByIdAsync(string id)
    {
      ValidateId(id);
      return _productService.GetAsync(id);
    }

    public async Task<Product?> GetByNameAsync(string name)
    {
      ValidateName(name);
      var products = await _productService.GetAllAsync();
      return products.FirstOrDefault(product =>
          StringHelpers.ToKebabCase(product.Name) == StringHelpers.ToKebabCase(name));
    }

    public Task<Product> CreateAsync(Product product)
    {
      return _productService.CreateAsync(product);
    }

    public Task<bool> UpdateAsync(string id, Product product)
    {
      ValidateId(id);
      product.Id = id; // Ensure the ID is correctly set for updates
      return _productService.UpdateAsync(id, product);
    }

    public Task<bool> DeleteAsync(string id)
    {
      ValidateId(id);
      return _productService.DeleteAsync(id);
    }

    public async Task<IEnumerable<string>> GetUniqueSizesAsync()
    {
      var products = await _productService.GetAllAsync();
      return ExtractUniqueSizes(products);
    }

    public async Task<IEnumerable<ColorDto>> GetUniqueColorsAsync()
    {
      var products = await _productService.GetAllAsync();
      return ExtractUniqueColors(products);
    }

    public async Task<Color?> GetColorByNameAsync(string name)
    {
      ValidateName(name);
      var filter = Builders<Color>.Filter.Eq(s => s.Name, name);
      return await _colorService.GetAsync(filter);
    }

    public async Task<Size?> GetSizeByNameAsync(string name)
    {
      ValidateName(name);
      var filter = Builders<Size>.Filter.Eq(s => s.Name, name);
      return await _sizeService.GetAsync(filter);
    }

    public async Task<Color> CreateColorAsync(Color color)
    {
      await _colorService.CreateAsync(color);
      return color;
    }

    public async Task<Size> CreateSizeAsync(Size size)
    {
      await _sizeService.CreateAsync(size);
      return size;
    }

    // Helper method to validate names
    private static void ValidateName(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        throw new ArgumentException("Name cannot be null or empty.", nameof(name));
    }

    // Helper method to extract unique sizes from product variants
    private static IEnumerable<string> ExtractUniqueSizes(IEnumerable<Product> products)
    {
      return products
          .SelectMany(p => p.Variants)
          .Select(v => v.Size.Name)
          .Distinct()
          .ToList();
    }

    // Helper method to extract unique colors with their hex codes
    private static IEnumerable<ColorDto> ExtractUniqueColors(IEnumerable<Product> products)
    {
      return products
          .SelectMany(p => p.Variants)
          .Select(v => new ColorDto { Id = v.Color.Id, Name = v.Color.Name, HexCode = v.Color.HexCode })
          .DistinctBy(c => c.Id)
          .ToList();
    }
  }
}
