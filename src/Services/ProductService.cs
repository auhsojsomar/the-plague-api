using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.Product;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using AutoMapper;

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
      // Validate product name uniqueness
      if (await _productRepository.GetByNameAsync(productDto.Name) != null)
        throw new ApplicationException("Product name already exists.");

      var product = new Product
      {
        Name = productDto.Name,
        Description = productDto.Description,
        Image = _mapper.Map<Image>(productDto.Image),
        Variants = await CreateVariantsAsync(productDto.Variants)
      };

      var createdProduct = await _productRepository.CreateAsync(product);
      return _mapper.Map<ProductDto>(createdProduct);
    }

    private async Task<List<Variant>> CreateVariantsAsync(IEnumerable<VariantDto> variantDtos)
    {
      var variants = new List<Variant>();

      foreach (var variantDto in variantDtos)
      {
        var size = await GetOrCreateSizeAsync(variantDto.Size.Name);
        var color = await GetOrCreateColorAsync(variantDto.Color.Name, variantDto.Color.HexCode);

        var newVariant = new Variant
        {
          Size = size,
          Color = color, // Reuse the color object
          Price = variantDto.Price,
          Quantity = variantDto.Quantity,
          Discount = variantDto.Discount != null
          ? new Discount
          {
            Type = variantDto.Discount.Type,
            Value = variantDto.Discount.Value
          }
          : null
        };

        variants.Add(newVariant);
      }

      return variants;
    }

    private async Task<Size> GetOrCreateSizeAsync(string sizeName)
    {
      var existingSize = await _productRepository.GetSizeByNameAsync(sizeName);
      if (existingSize != null) return existingSize;

      var newSize = new Size { Name = sizeName };
      await _productRepository.CreateSizeAsync(newSize);
      return newSize;
    }
    private async Task<Color> GetOrCreateColorAsync(string colorName, string hexCode)
    {
      // Check if the color already exists by its name
      var existingColor = await _productRepository.GetColorByNameAsync(colorName);
      if (existingColor != null)
      {
        // If it exists, return the existing color
        return existingColor;
      }

      // If it doesn't exist, create a new Color with the provided name and hex code
      var newColor = new Color
      {
        Name = colorName,
        HexCode = hexCode // Use the provided hex code for the new color
      };

      // Save the new color in the repository
      await _productRepository.CreateColorAsync(newColor);
      return newColor;
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
