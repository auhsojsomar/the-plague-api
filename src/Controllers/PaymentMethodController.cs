using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Services.Interfaces;

[Route("api/[controller]")]
[ApiController]
public class PaymentMethodController : ControllerBase
{
  private readonly IPaymentMethodService _paymentMethodService;

  public PaymentMethodController(IPaymentMethodService paymentMethodService)
  {
    _paymentMethodService = paymentMethodService;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<PaymentMethod>>> GetAllPaymentMethods()
  {
    var paymentMethods = await _paymentMethodService.GetAllPaymentMethodsAsync();
    return Ok(paymentMethods);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<PaymentMethod>> GetPaymentMethodById(string id)
  {
    var paymentMethod = await _paymentMethodService.GetPaymentMethodByIdAsync(id);
    return Ok(paymentMethod);
  }

  [HttpPost]
  public async Task<ActionResult<PaymentMethod>> CreatePaymentMethod([FromBody] PaymentMethod paymentMethod)
  {
    var createdPaymentMethod = await _paymentMethodService.CreatePaymentMethodAsync(paymentMethod);
    return CreatedAtAction(nameof(GetPaymentMethodById), new { id = createdPaymentMethod.Id }, createdPaymentMethod);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> UpdatePaymentMethod(string id, [FromBody] PaymentMethod paymentMethod)
  {
    var updated = await _paymentMethodService.UpdatePaymentMethodAsync(id, paymentMethod);
    return updated ? NoContent() : NotFound();
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> DeletePaymentMethod(string id)
  {
    var deleted = await _paymentMethodService.DeletePaymentMethodAsync(id);
    return deleted ? NoContent() : NotFound();
  }
}
