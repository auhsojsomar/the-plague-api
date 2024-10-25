using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
      _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      var products = await _productService.GetAllProductsAsync();
      return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
      var product = await _productService.GetProductByIdAsync(id);
      return product is not null
          ? Ok(product)
          : NotFound(new { Message = $"Product with ID {id} not found." });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ProductDto productDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var createdProduct = await _productService.CreateProductAsync(productDto);
      return CreatedAtAction(nameof(GetByIdAsync), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] ProductDto productDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var updated = await _productService.UpdateProductAsync(id, productDto);
      return updated
          ? NoContent()
          : NotFound(new { Message = $"Product with ID {id} not found." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
      var deleted = await _productService.DeleteProductAsync(id);
      return deleted
          ? NoContent()
          : NotFound(new { Message = $"Product with ID {id} not found." });
    }

    [HttpGet("sizes")]
    public async Task<IActionResult> GetSizesAsync()
    {
      var uniqueSizes = await _productService.GetUniqueSizesAsync();
      return Ok(uniqueSizes);
    }

    [HttpGet("colors")]
    public async Task<IActionResult> GetColorsAsync()
    {
      var uniqueColors = await _productService.GetUniqueColorsAsync();
      return Ok(uniqueColors);
    }
  }
}
