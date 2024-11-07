using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CartController : ControllerBase
  {
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
      _cartService = cartService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cart>> GetCartById(string id)
    {
      var cart = await _cartService.GetCartByIdAsync(id);
      if (cart == null)
        return NotFound();

      return Ok(cart);
    }

    [HttpGet("user/{userId}")]
    public async Task<ActionResult<Cart>> GetCartByUserId(string userId)
    {
      var cart = await _cartService.GetCartByUserIdAsync(userId);
      if (cart == null)
        return NotFound();

      return Ok(cart);
    }

    [HttpPost]
    public async Task<ActionResult<Cart>> CreateCart([FromBody] Cart cart)
    {
      var createdCart = await _cartService.CreateCartAsync(cart);
      return CreatedAtAction(nameof(GetCartById), new { id = createdCart.Id }, createdCart);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCart(string id, [FromBody] Cart cart)
    {
      var success = await _cartService.UpdateCartAsync(id, cart);
      if (!success)
        return NotFound();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCart(string id)
    {
      var success = await _cartService.DeleteCartAsync(id);
      if (!success)
        return NotFound();

      return NoContent();
    }

    [HttpPost("calculate-total")]
    public async Task<ActionResult<decimal>> CalculateTotalPrice([FromBody] List<CartItem> items)
    {
      var totalPrice = await _cartService.CalculateTotalPrice(items);
      return Ok(totalPrice);
    }
  }
}
