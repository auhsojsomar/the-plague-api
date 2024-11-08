using The_Plague_Api.Repositories.Interfaces;

namespace The_Plague_Api.Helpers
{
  public static class ValidationHelpers
  {
    public static void ValidateId(string id)
    {
      if (string.IsNullOrEmpty(id))
        throw new ArgumentException("ID cannot be null or empty.", nameof(id));
    }

    // Generic helper method to validate if an entity exists in a repository by ID
    public static async Task ValidateEntityExistenceAsync(string entityId, dynamic repository, string entityName)
    {
      if (string.IsNullOrEmpty(entityId))
      {
        throw new ArgumentException($"{entityName} ID cannot be null or empty.");
      }

      var entity = await repository.GetByIdAsync(entityId);
      if (entity == null)
      {
        throw new ArgumentException($"Invalid {entityName}: {entityName} does not exist.");
      }
    }

    // Method to validate user existence
    public static async Task ValidateUserAsync(string userId, IUserRepository userRepository)
    {
      await ValidateEntityExistenceAsync(userId, userRepository, "User");
    }

    // Method to validate product and variant existence
    public static async Task ValidateProductAndVariantAsync(string productId, string variantId, IProductRepository productRepository)
    {
      var product = await productRepository.GetByIdAsync(productId)
                    ?? throw new ArgumentException("Invalid ProductId: Product not found.");

      var variant = product.Variants.FirstOrDefault(v => v.Id == variantId);
      if (variant == null)
      {
        throw new ArgumentException("Invalid VariantId: Variant does not exist within the specified Product.");
      }
    }

  }
}