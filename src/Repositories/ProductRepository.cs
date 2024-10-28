using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Product;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Services;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using The_Plague_Api.Helpers;

namespace The_Plague_Api.Repositories
{
  public class ProductRepository : IProductRepository
  {
    private readonly IMongoDbService<Product> _productService;

    public ProductRepository(IMongoDatabase database)
    {
      const string productCollection = "products";
      _productService = new MongoDbService<Product>(database, productCollection);
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
      return _productService.GetAllAsync();
    }

    public Task<Product?> GetByIdAsync(string id)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException("ID cannot be null or empty.", nameof(id));

      return _productService.GetAsync(id);
    }

    public async Task<Product?> GetByNameAsync(string name)
    {
      if (string.IsNullOrWhiteSpace(name))
        return null; // Handle null or whitespace input

      // Retrieve all products from the repository (this could be optimized based on your requirements)
      var products = await _productService.GetAllAsync();

      // Filter the products in memory using kebab case comparison
      return products.FirstOrDefault(product =>
          StringHelpers.ToKebabCase(product.Name) == name);
    }

    public Task<Product> CreateAsync(Product product)
    {
      return _productService.CreateAsync(product);
    }

    public Task<bool> UpdateAsync(string id, Product product)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException("ID cannot be null or empty.", nameof(id));

      product.Id = id; // Ensure the ID is correctly set for updates
      return _productService.UpdateAsync(id, product);
    }

    public Task<bool> DeleteAsync(string id)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException("ID cannot be null or empty.", nameof(id));

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
          .Select(v => new ColorDto { Name = v.Color.Name, HexCode = v.Color.HexCode })
          .DistinctBy(c => c.Name)
          .ToList();
    }
  }
}
