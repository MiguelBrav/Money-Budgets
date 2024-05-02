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

                var id = await connection.QuerySingleAsync<int>("AccountType_Insert",
                    new { userId = account.UserId, name = account.Name }, 
                    commandType: System.Data.CommandType.StoredProcedure);

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

        public async Task<List<AccountTypeModel>> GetAccountsbyUser(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    var accounts = await connection.QueryAsync<AccountTypeModel>(
                        @"SELECT [Id],[Name],[UserId],[Order] FROM AccountType WHERE UserId = @UserId Order By [Order] Asc",
                        new { UserId = userId });

                    return accounts.ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task UpdateAccount(AccountTypeModel account)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    var accounts = await connection.ExecuteAsync(
                        @"Update AccountType Set [Name] = @Name 
                        FROM AccountType Where Id = @Id ",
                         account);

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<AccountTypeModel> GetAccountbyUserAndId(int userId, int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    var account = await connection.QueryFirstOrDefaultAsync<AccountTypeModel>(
                        @"SELECT [Id],[Name],[UserId],[Order] FROM AccountType WHERE UserId = @UserId
                        and Id = @Id",
                        new { UserId = userId, Id = id });

                    return account;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task DeleteAccount(int userId, int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    await connection.ExecuteAsync(
                        @"DELETE AccountType WHERE UserId = @UserId  and Id = @Id",
                        new { UserId = userId, Id = id });

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task OrderAccounts(IEnumerable<AccountTypeModel> accountTypeOrdered)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    var query = "UPDATE AccountType SET [Order] = @Order where Id = @Id";

                    await connection.ExecuteAsync(query, accountTypeOrdered);

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }
}
