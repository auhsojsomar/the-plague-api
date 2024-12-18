using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Models;

namespace The_Plague_Api.Services.Interfaces
{
  public interface ISalesReportService
  {
    public Task<object> GetSalesReport(SalesReportModel salesReportModel);
  }
}