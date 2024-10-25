using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  // This controller manages product-related operations.
  // Authorization is required for most endpoints, except where marked with [AllowAnonymous].
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductService _productService;

    // Constructor: Injects the product service to handle product operations.
    public ProductsController(IProductService productService)
    {
      _productService = productService;
    }

    // GET: api/products
    // Retrieves all products. No authorization required for this action.
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      var products = await _productService.GetAllProductsAsync();
      return Ok(products); // Returns a 200 OK with the list of products.
    }

    // GET: api/products/{id}
    // Retrieves a product by its ID. No authorization required.
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
      var product = await _productService.GetProductByIdAsync(id);
      return product is not null
          ? Ok(product) // Returns 200 OK if the product exists.
          : NotFound(new { Message = $"Product with ID {id} not found." }); // 404 if not found.
    }

    // POST: api/products
    // Creates a new product. Requires authorization.
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] ProductDto productDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState); // Returns 400 Bad Request if the model is invalid.

      var createdProduct = await _productService.CreateProductAsync(productDto);
      return CreatedAtAction(
          nameof(GetByIdAsync),
          new { id = createdProduct.Id },
          createdProduct
      ); // Returns 201 Created with the new product and location.
    }

    // PUT: api/products/{id}
    // Updates an existing product. Requires authorization.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] ProductDto productDto)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState); // 400 Bad Request for invalid model.

      var updated = await _productService.UpdateProductAsync(id, productDto);
      return updated
          ? NoContent() // 204 No Content on successful update.
          : NotFound(new { Message = $"Product with ID {id} not found." }); // 404 if not found.
    }

    // DELETE: api/products/{id}
    // Deletes a product by its ID. Requires authorization.
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
      var deleted = await _productService.DeleteProductAsync(id);
      return deleted
          ? NoContent() // 204 No Content on successful deletion.
          : NotFound(new { Message = $"Product with ID {id} not found." }); // 404 if not found.
    }

    // GET: api/products/sizes
    // Retrieves a list of unique product sizes. No authorization required.
    [AllowAnonymous]
    [HttpGet("sizes")]
    public async Task<IActionResult> GetSizesAsync()
    {
      var uniqueSizes = await _productService.GetUniqueSizesAsync();
      return Ok(uniqueSizes); // 200 OK with the list of sizes.
    }

    // GET: api/products/colors
    // Retrieves a list of unique product colors. No authorization required.
    [AllowAnonymous]
    [HttpGet("colors")]
    public async Task<IActionResult> GetColorsAsync()
    {
      var uniqueColors = await _productService.GetUniqueColorsAsync();
      return Ok(uniqueColors); // 200 OK with the list of colors.
    }
  }
}
