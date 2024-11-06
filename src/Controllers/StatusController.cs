using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class StatusController : ControllerBase
  {
    private readonly IStatusService _statusService;

    public StatusController(IStatusService statusService)
    {
      _statusService = statusService;
    }

    // GET: api/Status
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Status>>> GetAll()
    {
      var statuses = await _statusService.GetAllAsync();
      return Ok(statuses);
    }

    // GET: api/Status/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Status>> GetById(string id)
    {
      var status = await _statusService.GetByIdAsync(id);

      if (status == null)
      {
        return NotFound();
      }

      return Ok(status);
    }

    // POST: api/Status
    [HttpPost]
    public async Task<ActionResult<Status>> Create(Status status)
    {
      if (status == null)
      {
        return BadRequest("Status data is invalid.");
      }

      var createdStatus = await _statusService.CreateAsync(status);
      return CreatedAtAction(nameof(GetById), new { id = createdStatus.Id }, createdStatus);
    }

    // PUT: api/Status/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult> Update(string id, Status status)
    {
      var existingStatus = await _statusService.GetByIdAsync(id);
      if (existingStatus == null)
      {
        return NotFound();
      }

      var updated = await _statusService.UpdateAsync(id, status);
      if (updated)
      {
        return NoContent();
      }

      return BadRequest("Unable to update status.");
    }

    // DELETE: api/Status/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(string id)
    {
      var existingStatus = await _statusService.GetByIdAsync(id);
      if (existingStatus == null)
      {
        return NotFound();
      }

      var deleted = await _statusService.DeleteAsync(id);
      if (deleted)
      {
        return NoContent();
      }

      return BadRequest("Unable to delete status.");
    }
  }
}
