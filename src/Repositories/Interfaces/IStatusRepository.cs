using The_Plague_Api.Data.Entities.Order;

namespace The_Plague_Api.Repositories.Interfaces
{
  public interface IStatusRepository
  {
    Task<Status> CreateAsync(Status status);
    Task<IEnumerable<Status>> GetAllAsync();
    Task<Status> GetByIdAsync(string id);
    Task<bool> UpdateAsync(string id, Status status);
    Task<bool> DeleteAsync(string id);
  }
}
