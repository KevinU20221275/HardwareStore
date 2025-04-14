using HardwareStore.Data;
using HardwareStore.Models;
using Microsoft.AspNetCore.Identity;

namespace HardwareStore.Repositories.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public EmployeeRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _dataAccess.GetDataAsync<Employee, dynamic>(
                "dbo.spEmployee_GetAll",
                new { }
                );
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            var university = await _dataAccess.GetDataAsync<Employee, dynamic>(
                "dbo.spEmployee_GetById",
                new { Id = id }
                );

            return university.FirstOrDefault();
        }

        public async Task EditAsync(Employee employee)
        {
            
            await _dataAccess.SaveDataAsync(
                "dbo.spEmployee_Update",
                new
                {
                    employee.Id,
                    employee.FirstName,
                    employee.LastName,
                    employee.Username,
                    employee.RoleId,
                    employee.Phone,
                    employee.Email,
                    employee.Salary,
                    employee.Address,
                    employee.City
                }
                );
        }

        public async Task DeleteAsync(int id)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spEmployee_Delete",
                new { Id = id }
                );
        }

        public async Task AddAsync(Employee employee)
        {
            var hasher = new PasswordHasher<object>();
            employee.Password = hasher.HashPassword(null, employee.Password);

            await _dataAccess.SaveDataAsync(
                "dbo.spEmployee_Insert",
                new { employee.FirstName, 
                    employee.LastName,
                    employee.Username,
                    employee.Password,
                    employee.RoleId,
                    employee.Phone, 
                    employee.Email, 
                    employee.Salary, 
                    employee.Address, 
                    employee.City });
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await _dataAccess.GetDataAsync<Role, dynamic>(
                "dbo.spRole_GetAll",
                new { }
                );
        }

        public async Task<bool> UsernameAlredyExist(string username, int? id)
        {
            var employee = await _dataAccess.GetDataAsync<Employee, dynamic>(
                    "dbo.spEmployee_GetByUserName",
                    new { UserName = username }
                    );

            if (id != null)
            {
                return employee.FirstOrDefault().Id != id;
            }

            return employee.Any();
        } 

        public async Task UpdatePasswordAsync(UpdatePassword updatePassword)
        {
            var hasher = new PasswordHasher<object>();
            updatePassword.Password = hasher.HashPassword(null, updatePassword.Password);

            await _dataAccess.SaveDataAsync(
                "dbo.spEmployee_UpdatePassword",
                new {
                    updatePassword.Password,
                    updatePassword.Id
                }
                );
        }
    }
}
