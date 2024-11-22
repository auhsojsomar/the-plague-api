using The_Plague_Api.Repositories.Interfaces;

namespace The_Plague_Api.Helpers
{
  public static class StockHelpers
  {
    // Method to update the variant quantity based on the ordered quantity
    public static async Task UpdateVariantQuantityAsync(string productId, string variantId, int orderQuantity, IProductRepository productRepository)
    {
      var product = await productRepository.GetByIdAsync(productId)
                    ?? throw new ArgumentException("Invalid ProductId: Product not found.");

      var variant = product.Variants.FirstOrDefault(v => v.Id == variantId);
      if (variant == null)
      {
        throw new ArgumentException("Invalid VariantId: Variant does not exist within the specified Product.");
      }

      if (variant.Quantity < orderQuantity)
      {
        throw new ArgumentException("Order quantity exceeds available stock for the variant.");
      }

      // Update the variant quantity
      variant.Quantity -= orderQuantity;

      // Persist the updated product with the modified variant quantity
      await productRepository.UpdateAsync(productId, product);
    }
  }
}
