using HardwareStore.Data;
using HardwareStore.Models;

namespace HardwareStore.Repositories.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public RoleRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _dataAccess.GetDataAsync<Role, dynamic>(
                "dbo.spRole_GetAll",
                new { }
                );
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            var roles = await _dataAccess.GetDataAsync<Role, dynamic>(
                "dbo.spRole_GetById",
                new { Id = id }
                );

            return roles.FirstOrDefault();
        }

        public async Task EditAsync(Role role)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spRole_Update",
                new
                {
                    role.Id,
                    role.Name,
                }
                );
        }

        public async Task DeleteAsync(int id)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spRole_Delete",
                new { Id = id }
                );
        }

        public async Task AddAsync(Role role)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spRole_Insert",
                new { role.Name });
        }
    }
}
