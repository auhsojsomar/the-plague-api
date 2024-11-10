using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ShippingFeeController : ControllerBase
  {
    private readonly IShippingFeeService _shippingFeeService;

    public ShippingFeeController(IShippingFeeService shippingFeeService)
    {
      _shippingFeeService = shippingFeeService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShippingFee>>> GetAllShippingFees()
    {
      var shippingFees = await _shippingFeeService.GetAllShippingFeesAsync();
      return Ok(shippingFees);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShippingFee>> GetShippingFeeById(string id)
    {
      var shippingFee = await _shippingFeeService.GetShippingFeeByIdAsync(id);
      if (shippingFee == null)
        return NotFound();

      return Ok(shippingFee);
    }

    [HttpPost]
    public async Task<ActionResult<ShippingFee>> CreateShippingFee([FromBody] ShippingFee shippingFee)
    {
      var createdShippingFee = await _shippingFeeService.CreateShippingFeeAsync(shippingFee);
      return CreatedAtAction(nameof(GetShippingFeeById), new { id = createdShippingFee.Id }, createdShippingFee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShippingFee(string id, [FromBody] ShippingFee shippingFee)
    {
      var success = await _shippingFeeService.UpdateShippingFeeAsync(id, shippingFee);
      if (!success)
        return NotFound();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShippingFee(string id)
    {
      var success = await _shippingFeeService.DeleteShippingFeeAsync(id);
      if (!success)
        return NotFound();

      return NoContent();
    }
  }
}
