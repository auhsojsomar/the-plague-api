using AutoMapper;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _repository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
      var products = await _repository.GetAllAsync();
      return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(string id)
    {
      var product = await _repository.GetByIdAsync(id);
      return product != null ? _mapper.Map<ProductDto>(product) : null;
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
    {
      var product = _mapper.Map<Product>(productDto);
      await _repository.CreateAsync(product);
      return _mapper.Map<ProductDto>(product);
    }

    public async Task<bool> UpdateProductAsync(string id, ProductDto productDto)
    {
      var product = _mapper.Map<Product>(productDto);
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

    public async Task<IEnumerable<ColorDto>> GetUniqueColorsAsync()
    {
      var colors = await _repository.GetUniqueColorsAsync();
      return _mapper.Map<IEnumerable<ColorDto>>(colors);
    }
  }
}
