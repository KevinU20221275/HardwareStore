using HardwareStore.Models;

namespace HardwareStore.Repositories.Sales
{
    public interface ISaleRepository
    {
        Task AddAsync(Sale sale);
        Task<int> AddCustomerAsync(Customer customer);
        Task<IEnumerable<Sale>> GetAllAsync();
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<IEnumerable<Employee>> GetAllEmployeesAsync();
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Product?> GetProductByIdAsync(int id);
        Task<Sale?> GetSaleByIdAsync(int id);
        Task<IEnumerable<SaleDetail?>> GetSaleDetailsByIdAsync(int id);
    }
}
