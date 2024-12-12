using The_Plague_Api.Data.Entities;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class BannerService : IBannerService
  {
    private readonly IBannerRepository _bannerRepository;

    public BannerService(IBannerRepository bannerRepository)
    {
      _bannerRepository = bannerRepository;
    }

    public async Task<IEnumerable<Banner>> GetAllBannersAsync()
    {
      return await _bannerRepository.GetAllAsync();
    }

    public async Task<Banner?> GetBannerByIdAsync(string id)
    {
      return await _bannerRepository.GetByIdAsync(id);
    }

    public async Task<Banner> CreateBannerAsync(Banner banner)
    {
      return await _bannerRepository.CreateAsync(banner);
    }

    public async Task<bool> UpdateBannerAsync(string id, Banner banner)
    {
      return await _bannerRepository.UpdateAsync(id, banner);
    }

    public async Task<bool> DeleteBannerAsync(string id)
    {
      return await _bannerRepository.DeleteAsync(id);
    }
  }
}
