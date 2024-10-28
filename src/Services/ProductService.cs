using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.Product;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using AutoMapper;
using The_Plague_Api.Helpers;

namespace The_Plague_Api.Services
{
  public class ProductService : IProductService
  {
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
      var products = await _productRepository.GetAllAsync();
      return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetProductByIdAsync(string id)
    {
      var product = await _productRepository.GetByIdAsync(id);
      return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto?> GetProductByNameAsync(string name)
    {
      var product = await _productRepository.GetByNameAsync(name);
      return _mapper.Map<ProductDto>(product);
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto productDto)
    {
      if (await _productRepository.GetByNameAsync(productDto.Name) != null)
        throw new ApplicationException("Product name already exists.");

      var product = _mapper.Map<Product>(productDto);
      var createdProduct = await _productRepository.CreateAsync(product);
      return _mapper.Map<ProductDto>(createdProduct);
    }

    public async Task<bool> UpdateProductAsync(string id, ProductDto productDto)
    {
      var product = _mapper.Map<Product>(productDto);
      return await _productRepository.UpdateAsync(id, product);
    }

    public async Task<bool> DeleteProductAsync(string id)
    {
      return await _productRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<string>> GetUniqueSizesAsync()
    {
      return await _productRepository.GetUniqueSizesAsync();
    }

    public async Task<IEnumerable<ColorDto>> GetUniqueColorsAsync()
    {
      return await _productRepository.GetUniqueColorsAsync();
    }
  }
}
