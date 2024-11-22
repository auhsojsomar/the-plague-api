using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities.Product;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  // This controller manages CRUD operations for discounts.
  // Requires authorization by default, except for the endpoints marked with [AllowAnonymous].
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class DiscountsController : ControllerBase
  {
    private readonly IDiscountService _discountService;

    // Constructor: Injects the discount service to handle discount-related operations.
    public DiscountsController(IDiscountService discountService)
    {
      _discountService = discountService;
    }

    // GET: api/discounts
    // Retrieves all discounts. No authorization is required for this action.
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      var discounts = await _discountService.GetAllDiscountsAsync();
      return Ok(discounts); // Returns 200 OK with the list of discounts.
    }

    // GET: api/discounts/{id}
    // Retrieves a specific discount by ID. No authorization is required.
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
      var discount = await _discountService.GetDiscountByIdAsync(id);
      return discount is not null
          ? Ok(discount) // Returns 200 OK with the discount details if found.
          : NotFound(new { Message = $"Discount with ID {id} not found." }); // 404 if not found.
    }

    // POST: api/discounts
    // Creates a new discount. Requires authorization.
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] Discount discount)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState); // 400 Bad Request for invalid input.

      var createdDiscount = await _discountService.CreateDiscountAsync(discount);
      return CreatedAtAction(
          nameof(GetByIdAsync),
          new { id = createdDiscount.Id },
          createdDiscount
      ); // 201 Created with the newly created discount.
    }

    // PUT: api/discounts/{id}
    // Updates an existing discount. Requires authorization.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Discount discount)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState); // 400 Bad Request for invalid input.

      var updatedDiscount = await _discountService.UpdateDiscountAsync(id, discount);
      return updatedDiscount
          ? Ok(new { Message = "Discount updated successfully", Data = updatedDiscount }) // 200 OK on successful update.
          : NotFound(new { Message = $"Discount with ID {id} not found." }); // 404 if the discount doesn't exist.
    }

    // DELETE: api/discounts/{id}
    // Deletes a discount by its ID. Requires authorization.
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
      var deleted = await _discountService.DeleteDiscountAsync(id);
      return deleted
          ? NoContent() // 204 No Content on successful deletion.
          : NotFound(new { Message = $"Discount with ID {id} not found." }); // 404 if the discount doesn't exist.
    }
  }
}
