using Microsoft.AspNetCore.Mvc;
using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Models;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SalesReportController : ControllerBase
  {
    private readonly ISalesReportService _salesReportService;

    public SalesReportController(ISalesReportService salesReportService)
    {
      _salesReportService = salesReportService;
    }

    [HttpPost]
    public async Task<ActionResult<IEnumerable<SalesReportDto>>> GetSalesReport([FromBody] SalesReportModel salesReport)
    {
      return Ok(await _salesReportService.GetSalesReport(salesReport));
    }
  }
}