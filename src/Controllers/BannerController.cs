using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Entities;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class BannerController : ControllerBase
  {
    private readonly IBannerService _bannerService;

    public BannerController(IBannerService bannerService)
    {
      _bannerService = bannerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Banner>>> GetAllBanners()
    {
      var banners = await _bannerService.GetAllBannersAsync();
      return Ok(banners);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Banner>> GetBannerById(string id)
    {
      var banner = await _bannerService.GetBannerByIdAsync(id);
      if (banner == null)
        return NotFound();

      return Ok(banner);
    }

    [HttpPost]
    public async Task<ActionResult<Banner>> CreateBanner([FromBody] Banner banner)
    {
      var createdBanner = await _bannerService.CreateBannerAsync(banner);
      return CreatedAtAction(nameof(GetBannerById), new { id = createdBanner.Id }, createdBanner);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBanner(string id, [FromBody] Banner banner)
    {
      var success = await _bannerService.UpdateBannerAsync(id, banner);
      if (!success)
        return NotFound();

      return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBanner(string id)
    {
      var success = await _bannerService.DeleteBannerAsync(id);
      if (!success)
        return NotFound();

      return NoContent();
    }
  }
}
