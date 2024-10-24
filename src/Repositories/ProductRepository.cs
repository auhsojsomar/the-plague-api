using MongoDB.Driver;
using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Data.Dto;

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

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
      return await _productService.GetAllAsync();
    }

    public async Task<Product?> GetByIdAsync(string id)
    {
      return await _productService.GetAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
      return await _productService.CreateAsync(product);
    }

    public async Task<bool> UpdateAsync(string id, Product product)
    {
      product.Id = id; // Ensure the product ID is set correctly for updates
      return await _productService.UpdateAsync(id, product);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      return await _productService.DeleteAsync(id);
    }

    public async Task<IEnumerable<string>> GetUniqueSizesAsync()
    {
      try
      {
        var products = await _productService.GetAllAsync();

        // Extract unique size IDs from variants
        var uniqueSizes = products
            .SelectMany(product => product.Variants)
            .Select(variant => variant.Size.Name)
            .Distinct()
            .ToList();

        return uniqueSizes;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error in GetUniqueSizesAsync: {ex.Message}");
        throw;
      }
    }

    public async Task<IEnumerable<ColorDto>> GetUniqueColorsAsync()
    {
      try
      {
        // Fetch documents as Product objects directly
        var products = await _productService.GetAllAsync();

        // Extract unique colors and their hex codes from the product variants
        var uniqueColors = products
            .SelectMany(product => product.Variants) // Flatten the variants
            .Select(variant => new ColorDto
            {
              Name = variant.Color.Name, // Get the color name
              HexCode = variant.Color.HexCode // Get the hex code
            })
            .DistinctBy(c => c.Name) // Ensure distinct color names
            .ToList(); // Convert to a list

        return uniqueColors;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error in GetUniqueColorsAsync: {ex.Message}");
        throw;
      }
    }
  }
}
