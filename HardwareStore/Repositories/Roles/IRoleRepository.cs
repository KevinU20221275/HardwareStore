using HardwareStore.Models;

namespace HardwareStore.Repositories.Roles
{
    public interface IRoleRepository
    {
        Task AddAsync(Role roles);
        Task DeleteAsync(int id);
        Task EditAsync(Role roles);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> GetByIdAsync(int id);
    }
}
