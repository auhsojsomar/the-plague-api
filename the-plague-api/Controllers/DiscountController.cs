using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class DiscountsController : ControllerBase
  {
    private readonly IDiscountService _service;

    public DiscountsController(IDiscountService service)
    {
      _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var discounts = await _service.GetAllDiscountsAsync();
      return Ok(discounts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
      var discount = await _service.GetDiscountByIdAsync(id);
      if (discount == null) return NotFound();
      return Ok(discount);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Discount discount)
    {
      if (!ModelState.IsValid) return BadRequest(ModelState);
      var createdDiscount = await _service.CreateDiscountAsync(discount);
      return CreatedAtAction(nameof(GetById), new { id = createdDiscount.Id }, createdDiscount);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
      var deleted = await _service.DeleteDiscountAsync(id);
      if (!deleted) return NotFound();
      return NoContent();
    }
  }
}
