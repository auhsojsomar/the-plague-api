using The_Plague_Api.Data.Entities.Order;
using The_Plague_Api.Repositories.Interfaces;
using The_Plague_Api.Services.Interfaces;

namespace The_Plague_Api.Services
{
  public class StatusService : IStatusService
  {
    private readonly IStatusRepository _statusRepository;

    public StatusService(IStatusRepository statusRepository)
    {
      _statusRepository = statusRepository;
    }

    public async Task<Status> CreateAsync(Status status)
    {
      status.DateCreated = DateTime.UtcNow;
      return await _statusRepository.CreateAsync(status);
    }

    public async Task<IEnumerable<Status>> GetAllAsync()
    {
      return await _statusRepository.GetAllAsync();
    }

    public async Task<Status> GetByIdAsync(string id)
    {
      return await _statusRepository.GetByIdAsync(id);
    }

    public async Task<bool> UpdateAsync(string id, Status status)
    {
      status.DateModified = DateTime.UtcNow;
      return await _statusRepository.UpdateAsync(id, status);
    }

    public async Task<bool> DeleteAsync(string id)
    {
      return await _statusRepository.DeleteAsync(id);
    }
  }
}
