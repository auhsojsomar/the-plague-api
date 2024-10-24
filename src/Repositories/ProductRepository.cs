using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using MongoDB.Driver;
using The_Plague_Api.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace The_Plague_Api.Repositories
{
  public class ProductRepository : IProductRepository
  {
    private readonly IMongoDbService<Product> _productService;

    public ProductRepository(IMongoDatabase database)
    {
      const string collectionName = "products";
      _productService = new MongoDbService<Product>(database, collectionName);
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
      // Set Product ID
      product.Id = ObjectId.GenerateNewId().ToString();

      // Assign IDs to variants, sizes, and colors
      foreach (var variant in product.Variants)
      {
        variant.Id = ObjectId.GenerateNewId().ToString();
      }

      return await _productService.CreateAsync(product);
    }

    public async Task<bool> UpdateAsync(string id, Product product)
    {
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
        // Fetch documents as Product objects directly
        var products = await _productService.GetAllAsync();

        // Extract unique sizes from the product variants
        var uniqueSizes = products
            .SelectMany(product => product.Variants) // Flatten the variants
            .Select(variant => variant.Size.Name) // Get the size name
            .Distinct() // Get distinct sizes
            .ToList(); // Convert to a list

        return uniqueSizes;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Error in GetUniqueSizesAsync: {ex.Message}");
        throw;
      }
    }

    public async Task<IEnumerable<Color>> GetUniqueColorsAsync()
    {
      try
      {
        // Fetch documents as Product objects directly
        var products = await _productService.GetAllAsync();

        // Extract unique colors and their hex codes from the product variants
        var uniqueColors = products
            .SelectMany(product => product.Variants) // Flatten the variants
            .Select(variant => new Color
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
