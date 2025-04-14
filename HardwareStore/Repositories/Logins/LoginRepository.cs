using HardwareStore.Data;
using HardwareStore.Models;
using Microsoft.AspNetCore.Identity;

namespace HardwareStore.Repositories.Logins
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ISqlDataAccess _dataAccess;

        public LoginRepository(ISqlDataAccess dataAccess) 
        { 
            _dataAccess = dataAccess;
        }

        public async Task<Employee?> Login(string username, string password)
        {
            try
            {
                var employee = await _dataAccess.GetDataAsync<Employee, dynamic>(
                    "dbo.spEmployee_GetByUserName",
                    new { UserName = username }
                    );

                var user = employee.FirstOrDefault();

                if (user == null) return null;

                var hasher = new PasswordHasher<object>();
                var result = hasher.VerifyHashedPassword(null, user.Password, password);

                if (result == PasswordVerificationResult.Success || result == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    return user;
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


        public Task Logout()
        {
            throw new NotImplementedException();
        }
    }
}
