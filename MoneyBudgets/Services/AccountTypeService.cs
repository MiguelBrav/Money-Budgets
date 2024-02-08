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

        public async Task AddAccountType(AccountTypeModel account)
        {
            SqlConnection connection = new SqlConnection(_connectionString);

            try
            {
                connection.Open();

                var id = await connection.QuerySingleAsync<int>(
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

        public async Task<bool> ExistsAccount(string accountName, int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    var exists = await connection.QueryFirstOrDefaultAsync<int>(
                        @"SELECT 1 FROM AccountType WHERE Name = @Name AND UserId = @UserId",
                        new { Name = accountName, UserId = userId });

                    return exists == 1;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
