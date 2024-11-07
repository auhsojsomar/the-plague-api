using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class OrderStatusController : ControllerBase
  {
    private readonly IOrderStatusService _orderStatusService;

    public OrderStatusController(IOrderStatusService statusService)
    {
      _orderStatusService = statusService;
    }

    // GET: api/OrderStatus
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderStatus>>> GetAll()
    {
      var statuses = await _orderStatusService.GetAllAsync();
      return Ok(statuses);
    }

    // GET: api/OrderStatus/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderStatus>> GetById(string id)
    {
      var status = await _orderStatusService.GetByIdAsync(id);

      if (status == null)
      {
        return NotFound();
      }

      return Ok(status);
    }

    // POST: api/OrderStatus
    [HttpPost]
    public async Task<ActionResult<OrderStatus>> Create(OrderStatus status)
    {
      if (status == null)
      {
        return BadRequest("OrderStatus data is invalid.");
      }

      var createdStatus = await _orderStatusService.CreateAsync(status);
      return CreatedAtAction(nameof(GetById), new { id = createdStatus.Id }, createdStatus);
    }

    // PUT: api/OrderStatus/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, OrderStatus status)
    {
      var existingStatus = await _orderStatusService.GetByIdAsync(id);
      if (existingStatus == null)
      {
        return NotFound();
      }

      var updated = await _orderStatusService.UpdateAsync(id, status);
      if (updated)
      {
        return NoContent();
      }

      return BadRequest("Unable to update status.");
    }

    // DELETE: api/OrderStatus/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
      var existingStatus = await _orderStatusService.GetByIdAsync(id);
      if (existingStatus == null)
      {
        return NotFound();
      }

      var deleted = await _orderStatusService.DeleteAsync(id);
      if (deleted)
      {
        return NoContent();
      }

      return BadRequest("Unable to delete status.");
    }
  }
}
