using Dapper;
using Microsoft.Data.SqlClient;
using MoneyBudgets.Interfaces;
using MoneyBudgets.Models;

namespace MoneyBudgets.Services
{
    public class AccountTypeService : IAccountTypeService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;


        public AccountTypeService(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlServerConnection");
        }

        public void AddAccountType(AccountTypeModel account)
        {
            SqlConnection connection = new SqlConnection(_connectionString); 

            try
            {               
                connection.Open();

                var id = connection.QuerySingle<int>(
                    @"INSERT INTO AccountType ([Name], [UserId], [Order]) VALUES (@Name, @UserId, 0); SELECT SCOPE_IDENTITY();", account);

                account.Id = id;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Dispose();
            }

        }
    }
}
