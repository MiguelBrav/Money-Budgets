using Dapper;
using Microsoft.Data.SqlClient;
using MoneyBudgets.Extensions;
using MoneyBudgets.Interfaces;
using MoneyBudgets.Models;
using System.Data;

namespace MoneyBudgets.Services
{
    public class AccountTypeService : IAccountTypeService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly string _dbType;
        private readonly CreateConnection _createConnection;

        public AccountTypeService(IConfiguration configuration)
        {
            _createConnection = new CreateConnection();
            _configuration = configuration;
            //_connectionString = _configuration.GetConnectionString("SqlServerConnection");
            _dbType = _configuration.GetValue<string>("DatabaseType");
            if (_dbType == "SqlServer")
            {
                _connectionString = _configuration.GetConnectionString("SqlServerConnection");
            }
            else if (_dbType == "MySql")
            {
                _connectionString = _configuration.GetConnectionString("MySqlConnection");
            }
        }

        public async Task AddAccountType(AccountTypeModel account)
        {
            //SqlConnection connection = new SqlConnection(_connectionString);
            IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType);

            try
            {
                connection.Open();

                //var id = await connection.QuerySingleAsync<int>("AccountType_Insert",
                //    new { userId = account.UserId, name = account.Name }, 
                //    commandType: System.Data.CommandType.StoredProcedure);

                //account.Id = id;

                if (_dbType == "SqlServer")
                {
                    var id = await connection.QuerySingleAsync<int>("AccountType_Insert",
                        new { userId = account.UserId, name = account.Name },
                        commandType: CommandType.StoredProcedure);

                    account.Id = id;
                }
                else if (_dbType == "MySql")
                {
                    var id = await connection.QuerySingleAsync<int>("CALL AccountType_Insert(@userId, @name);",
                        new { userId = account.UserId, name = account.Name });

                    account.Id = id;
                }
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
            //using (SqlConnection connection = new SqlConnection(_connectionString))
            using (IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType))
            {
                try
                {
                    //await connection.OpenAsync();
                    connection.Open();

                    //var exists = await connection.QueryFirstOrDefaultAsync<int>(
                    //    @"SELECT 1 FROM AccountType WHERE Name = @Name AND UserId = @UserId",
                    //    new { Name = accountName, UserId = userId });

                    //return exists == 1;

                    string query = @"SELECT CASE WHEN EXISTS (
                                SELECT 1 FROM AccountType WHERE Name = @Name AND UserId = @UserId
                            ) THEN 1 ELSE 0 END";

                    var exists = await connection.QueryFirstOrDefaultAsync<int>(
                        query,
                        new { Name = accountName, UserId = userId }
                    );

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
            //using (SqlConnection connection = new SqlConnection(_connectionString))
            using (IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType))
            {
                try
                {
                    //await connection.OpenAsync();
                    connection.Open();

                    //var accounts = await connection.QueryAsync<AccountTypeModel>(
                    //    @"SELECT [Id],[Name],[UserId],[Order] FROM AccountType WHERE UserId = @UserId Order By [Order] Asc",
                    //    new { UserId = userId });

                    //return accounts.ToList();

                    string query = string.Empty;

                    if (_dbType == "SqlServer")
                    {
                        query = @"SELECT Id, Name, UserId, [Order] 
                          FROM AccountType 
                          WHERE UserId = @UserId 
                          ORDER BY [Order] ASC";
                    }
                    else if (_dbType == "MySql")
                    {
                        query = @"SELECT Id, Name, UserId, `Order` 
                          FROM AccountType 
                          WHERE UserId = @UserId 
                          ORDER BY `Order` ASC";
                    }

                    var accounts = await connection.QueryAsync<AccountTypeModel>(
                        query,
                        new { UserId = userId }
                    );

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
            //using (SqlConnection connection = new SqlConnection(_connectionString))
            using (IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType))
            {
                try
                {
                    //await connection.OpenAsync();
                    connection.Open();

                    var accounts = await connection.ExecuteAsync(
                        @"Update AccountType Set Name = @Name 
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
            //using (SqlConnection connection = new SqlConnection(_connectionString))
            using (IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType))
            {
                try
                {
                    //await connection.OpenAsync();
                    connection.Open();

                    //var account = await connection.QueryFirstOrDefaultAsync<AccountTypeModel>(
                    //    @"SELECT [Id],[Name],[UserId],[Order] FROM AccountType WHERE UserId = @UserId
                    //    and Id = @Id",
                    //    new { UserId = userId, Id = id });

                    //return account;

                    string query = string.Empty;

                    if (_dbType == "SqlServer")
                    {
                        query = @"SELECT Id, Name, UserId, [Order] 
                          FROM AccountType 
                          WHERE UserId = @UserId AND Id = @Id";
                    }
                    else if (_dbType == "MySql")
                    {
                        query = @"SELECT Id, Name, UserId, `Order` 
                          FROM AccountType 
                          WHERE UserId = @UserId AND Id = @Id";
                    }  

                    var account = await connection.QueryFirstOrDefaultAsync<AccountTypeModel>(
                        query,
                        new { UserId = userId, Id = id }
                    );

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
            //using (SqlConnection connection = new SqlConnection(_connectionString))
            using (IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType))
            {
                try
                {
                    //await connection.OpenAsync();
                    connection.Open();

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
            //using (SqlConnection connection = new SqlConnection(_connectionString))
            using (IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType))
            {
                try
                {
                    //await connection.OpenAsync();
                    connection.Open();

                    //var query = "UPDATE AccountType SET [Order] = @Order where Id = @Id";

                    //await connection.ExecuteAsync(query, accountTypeOrdered);

                    string query = string.Empty;

                    if (_dbType == "SqlServer")
                    {
                        query = "UPDATE AccountType SET [Order] = @Order WHERE Id = @Id";
                    }
                    else if (_dbType == "MySql")
                    {
                        query = "UPDATE AccountType SET `Order` = @Order WHERE Id = @Id";
                    }

                    foreach (var accountType in accountTypeOrdered)
                    {
                        await connection.ExecuteAsync(query, new { Order = accountType.Order, Id = accountType.Id });
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }
}
