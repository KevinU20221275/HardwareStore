using HardwareStore.Data;
using HardwareStore.Models;

namespace HardwareStore.Repositories.Customers
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public CustomerRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _dataAccess.GetDataAsync<Customer, dynamic>(
                "dbo.spCustomer_GetAll",
                new { }
                );
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            var university = await _dataAccess.GetDataAsync<Customer, dynamic>(
                "dbo.spCustomer_GetById",
                new { Id = id }
                );

            return university.FirstOrDefault();
        }

        public async Task EditAsync(Customer customer)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spCustomer_Update",
                new
                {
                    customer.Id,
                    customer.FirstName,
                    customer.LastName,
                    customer.Email,
                    customer.Phone,
                    customer.Address,
                    customer.City
                }
                );
        }

        public async Task DeleteAsync(int id)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spCustomer_Delete",
                new { Id = id }
                );
        }

        public async Task AddAsync(Customer customer)
        {
            await _dataAccess.SaveDataAsync(
                "dbo.spCustomer_Insert",
                new { customer.FirstName, customer.LastName, customer.Email, customer.Phone, customer.Address, customer.City });
        }
    }
}
