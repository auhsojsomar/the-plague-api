using The_Plague_Api.Data.Entities.Product;

namespace The_Plague_Api.Helpers
{
  public static class PriceHelpers
  {
    public static decimal CalculateSalePrice(decimal price, DiscountType? discountType, decimal? discountValue)
    {
      if (discountType == null || discountValue == null) return price;

      return discountType switch
      {
        DiscountType.Percentage => price - (price * (discountValue.Value / 100)),
        DiscountType.FixedAmount => price - discountValue.Value,
        _ => price // Fallback to original price if discount type is unknown
      };
    }
  }
}
