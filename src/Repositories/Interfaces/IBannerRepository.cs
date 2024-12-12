using The_Plague_Api.Data.Entities;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IBannerRepository
  {
    Task<IEnumerable<Banner>> GetAllAsync();
    Task<Banner?> GetByIdAsync(string id);
    Task<Banner> CreateAsync(Banner banner);
    Task<bool> UpdateAsync(string id, Banner banner);
    Task<bool> DeleteAsync(string id);
  }
}