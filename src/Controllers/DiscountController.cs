using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class DiscountsController : ControllerBase
  {
    private readonly IDiscountService _discountService;

    public DiscountsController(IDiscountService discountService)
    {
      _discountService = discountService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
      var discounts = await _discountService.GetAllDiscountsAsync();
      return Ok(discounts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
      var discount = await _discountService.GetDiscountByIdAsync(id);
      return discount is not null
          ? Ok(discount)
          : NotFound(new { Message = $"Discount with ID {id} not found." });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] Discount discount)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var createdDiscount = await _discountService.CreateDiscountAsync(discount);
      return CreatedAtAction(nameof(GetByIdAsync), new { id = createdDiscount.Id }, createdDiscount);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] Discount discount)
    {
      if (!ModelState.IsValid)
        return BadRequest(ModelState);

      var updatedDiscount = await _discountService.UpdateDiscountAsync(id, discount);
      return updatedDiscount
          ? Ok(new { Message = "Discount updated successfully", Data = updatedDiscount })
          : NotFound(new { Message = $"Discount with ID {id} not found." });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
      var deleted = await _discountService.DeleteDiscountAsync(id);
      return deleted
          ? NoContent()
          : NotFound(new { Message = $"Discount with ID {id} not found." });
    }
  }
}
