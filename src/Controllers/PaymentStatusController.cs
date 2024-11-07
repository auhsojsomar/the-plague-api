using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PaymentStatusController : ControllerBase
  {
    private readonly IPaymentStatusService _paymentStatusService;

    public PaymentStatusController(IPaymentStatusService paymentStatusService)
    {
      _paymentStatusService = paymentStatusService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentStatus>>> GetAllPaymentStatuses()
    {
      var paymentStatuses = await _paymentStatusService.GetAllPaymentStatusesAsync();
      return Ok(paymentStatuses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PaymentStatus>> GetPaymentStatusById(string id)
    {
      var paymentStatus = await _paymentStatusService.GetPaymentStatusByIdAsync(id);
      if (paymentStatus == null)
        return NotFound();

      return Ok(paymentStatus);
    }

    [HttpPost]
    public async Task<ActionResult<PaymentStatus>> CreatePaymentStatus([FromBody] PaymentStatus paymentStatus)
    {
      var createdPaymentStatus = await _paymentStatusService.CreatePaymentStatusAsync(paymentStatus);
      return CreatedAtAction(nameof(GetPaymentStatusById), new { id = createdPaymentStatus.Id }, createdPaymentStatus);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePaymentStatus(string id, [FromBody] PaymentStatus paymentStatus)
    {
      var success = await _paymentStatusService.UpdatePaymentStatusAsync(id, paymentStatus);
      if (!success)
        return NotFound();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePaymentStatus(string id)
    {
      var success = await _paymentStatusService.DeletePaymentStatusAsync(id);
      if (!success)
        return NotFound();

      return NoContent();
    }
  }
}
