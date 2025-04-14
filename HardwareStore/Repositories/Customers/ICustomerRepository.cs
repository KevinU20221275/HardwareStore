using HardwareStore.Models;

namespace HardwareStore.Repositories.Customers
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer customer);
        Task DeleteAsync(int id);
        Task EditAsync(Customer customer);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
    }
}
