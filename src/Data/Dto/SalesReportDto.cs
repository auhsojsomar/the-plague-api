using The_Plague_Api.Data.Entities.Product;

namespace The_Plague_Api.Data.Dto
{
  public class SalesReportDto
  {
    public required string ProductName { get; set; }

    public required string VariantName { get; set; }

    public int QuantitySold { get; set; }

    public decimal UnitPrice { get; set; }

    public DiscountType? DiscountType { get; set; }

    public decimal? DiscountValue { get; set; }

    public decimal? Total { get; set; }

    public DateTime DateCreated { get; set; }

  }
}