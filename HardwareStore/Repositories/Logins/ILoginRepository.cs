using HardwareStore.Models;

namespace HardwareStore.Repositories.Logins
{
    public interface ILoginRepository
    {
        Task<Employee?> Login(string username, string password);

        Task Logout();
    }
}
