using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Collections;

namespace The_Plague_Api.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
      _repository = repository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
      return await _repository.GetAllAsync();
    }

    public async Task<Product?> GetProductByIdAsync(string id)
    {
      return await _repository.GetByIdAsync(id);
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
      return await _repository.CreateAsync(product);
    }

    public async Task<bool> UpdateProductAsync(string id, Product product)
    {
      return await _repository.UpdateAsync(id, product);
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
      return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<string>> GetUniqueSizesAsync()
    {
      return await _repository.GetUniqueSizesAsync();
    }

    public async Task<IEnumerable<Color>> GetUniqueColorsAsync()
    {
      return await _repository.GetUniqueColorsAsync();
    }
  }
}
