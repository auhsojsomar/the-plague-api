using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
      _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var products = await _service.GetAllProductsAsync();
      return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
      var product = await _service.GetProductByIdAsync(id);
      if (product == null) return NotFound();

      return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var createdProduct = await _service.CreateProductAsync(product);
      return CreatedAtAction(nameof(GetById), new { id = createdProduct.Id }, createdProduct);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Product product)
    {
      if (!ModelState.IsValid) return BadRequest(ModelState);

      var updated = await _service.UpdateProductAsync(id, product);
      if (!updated) return NotFound();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      var deleted = await _service.DeleteProductAsync(id);
      if (!deleted) return NotFound();

      return NoContent();
    }

    [HttpGet("sizes")]
    public async Task<IActionResult> GetSizes()
    {
      var uniqueSizes = await _service.GetUniqueSizesAsync();
      return Ok(uniqueSizes);
    }

    [HttpGet("colors")]
    public async Task<IActionResult> GetColors()
    {
      var uniqueColors = await _service.GetUniqueColorsAsync();
      return Ok(uniqueColors);
    }
  }
}
