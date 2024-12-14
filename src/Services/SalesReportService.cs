using The_Plague_Api.Data.Dto;
using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;
using The_Plague_Api.Helpers;
using The_Plague_Api.Data.Models;

namespace The_Plague_Api.Services
{
  public class SalesReportService : ISalesReportService
  {
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public SalesReportService(IOrderRepository orderRepository, IProductRepository productRepository)
    {
      _orderRepository = orderRepository;
      _productRepository = productRepository;
    }

    public async Task<object> GetSalesReport(SalesReportModel salesReportModel)
    {
      // Fetch all relevant orders
      var filteredOrders = await GetFilteredOrdersAsync(salesReportModel);

      // Generate sales data from orders
      var salesData = new List<SalesReportDto>();
      foreach (var order in filteredOrders)
      {
        salesData.AddRange(await GetSalesDataFromOrderAsync(order));
      }

      // Aggregate the data and calculate the grand total
      var aggregatedData = AggregateSalesData(salesData);
      var grandTotal = aggregatedData.Sum(sale => sale.Total);

      // Wrap in desired structure
      return new
      {
        data = aggregatedData,
        grandTotal
      };
    }

    private async Task<IEnumerable<Order>> GetFilteredOrdersAsync(SalesReportModel salesReportModel)
    {
      var orders = await _orderRepository.GetAllAsync();

      var dateFrom = salesReportModel.DateFrom; // 2024-12-01 00:00:00
      var dateTo = salesReportModel.DateTo.AddDays(1).AddMilliseconds(-1); // 2024-12-15 23:59:59.999

      return orders.Where(order => order.DateCreated >= dateFrom &&
                                   order.DateCreated <= dateTo &&
                                   order.PaymentStatusKey == 2); // Assuming 2 is 'Paid'
    }

    private async Task<IEnumerable<SalesReportDto>> GetSalesDataFromOrderAsync(Order order)
    {
      var salesData = new List<SalesReportDto>();

      foreach (var item in order.Items)
      {
        var product = await _productRepository.GetByIdAsync(item.ProductId);
        var variant = product?.Variants.FirstOrDefault(v => v.Id == item.VariantId);

        if (product != null && variant != null)
        {
          // Calculate the sale price using PriceHelpers
          var discountedPrice = PriceHelpers.CalculateSalePrice(
              variant.Price,
              variant.Discount?.Type,
              variant.Discount?.Value
          );

          // Add to sales data
          salesData.Add(new SalesReportDto
          {
            ProductName = product.Name,
            VariantName = $"{variant.Color.Name}, {variant.Size.Name}",
            QuantitySold = item.Quantity,
            UnitPrice = variant.Price,
            Total = discountedPrice * item.Quantity,
            DiscountType = variant.Discount?.Type,
            DiscountValue = variant.Discount?.Value,
            DateCreated = order.DateCreated
          });
        }
      }

      return salesData;
    }

    private IEnumerable<SalesReportDto> AggregateSalesData(IEnumerable<SalesReportDto> salesData)
    {
      return salesData
          .GroupBy(sale => new { sale.ProductName, sale.VariantName, sale.UnitPrice, sale.DiscountType })
          .Select(group => new SalesReportDto
          {
            ProductName = group.Key.ProductName,
            VariantName = group.Key.VariantName,
            QuantitySold = group.Sum(x => x.QuantitySold),
            UnitPrice = group.Key.UnitPrice,
            Total = group.Sum(x => x.Total),
            DiscountType = group.Key.DiscountType,
            DiscountValue = group.First().DiscountValue,
            DateCreated = group.Min(x => x.DateCreated)
          });
    }
  }
}
