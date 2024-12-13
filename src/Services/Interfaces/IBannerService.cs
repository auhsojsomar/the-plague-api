using The_Plague_Api.Data.Entities.Banner;

namespace The_Plague_Api.Services.Interfaces
{
  public interface IBannerService
  {
    Task<IEnumerable<Banner>> GetAllBannersAsync();
    Task<Banner?> GetBannerByIdAsync(string id);
    Task<Banner> CreateBannerAsync(Banner banner);
    Task<bool> UpdateBannerAsync(string id, Banner banner);
    Task<bool> DeleteBannerAsync(string id);
  }
}
