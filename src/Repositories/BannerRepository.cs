using MongoDB.Driver;
using The_Plague_Api.Data.Entities.Banner;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Repositories
{
  public class BannerRepository : IBannerRepository
  {
    private readonly IMongoDbService<Banner> _bannerService;
    private readonly IMongoCollection<Banner> _bannerCollection;

    public BannerRepository(IMongoDatabase database)
    {
      const string bannerCollection = "banner";

      _bannerService = new MongoDbService<Banner>(database, bannerCollection);
      _bannerCollection = database.GetCollection<Banner>(bannerCollection);
    }

    public async Task<IEnumerable<Banner>> GetAllAsync()
    {
      return await _bannerService.GetAllAsync();
    }

    public async Task<IEnumerable<Banner>> GetMainBannerAsync()
    {
      var filter = Builders<Banner>.Filter.Eq(x => x.BannerType, BannerType.Main);
      return await _bannerService.GetAllAsync(filter);
    }

    public async Task<IEnumerable<Banner>> GetProductBannerAsync()
    {
      var filter = Builders<Banner>.Filter.Eq(x => x.BannerType, BannerType.Product);
      return await _bannerService.GetAllAsync(filter);
    }

    public async Task<Banner?> GetByIdAsync(string id)
    {
      return await _bannerService.GetAsync(id);
    }

    public async Task<Banner> CreateAsync(Banner banner)
    {
      await _bannerService.CreateAsync(banner);
      return banner;
    }

    public async Task<bool> UpdateAsync(string id, Banner banner)
    {
      banner.Id = id;
      banner.DateModified = DateTime.UtcNow;
      return await _bannerService.UpdateAsync(id, banner);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      return await _bannerService.DeleteAsync(id);
    }
  }
}
