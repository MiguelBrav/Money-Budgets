using Dapper;
using Microsoft.Data.SqlClient;
using MoneyBudgets.Extensions;
using MoneyBudgets.Interfaces;
using MoneyBudgets.Models;
using System.Data;

namespace MoneyBudgets.Services
{
    public class AccountService : IAccountService
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly string _dbType;
        private readonly CreateConnection _createConnection;

        public AccountService(IConfiguration configuration)
        {
            _createConnection = new CreateConnection();
            _configuration = configuration;
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

        public async Task AddAccount(AccountModel account)
        {
            IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType);

            try
            {
                connection.Open();

                string insertQuery = string.Empty;

                if (_dbType == "SqlServer")
                {
                    insertQuery = @"INSERT INTO 
                    Account (Name, AccountTypeId, Balance, Description)
                    VALUES (@Name, @AccountTypeId, @Balance, @Description);
                    SELECT SCOPE_IDENTITY()";
                }
                else if (_dbType == "MySql")
                {
                    insertQuery = @"INSERT INTO 
                    Account (Name, AccountTypeId, Balance, Description)
                    VALUES (@Name, @AccountTypeId, @Balance, @Description);
                    SELECT LAST_INSERT_ID()";
                }

                int accountId = await connection.QuerySingleAsync<int>(insertQuery, new
                {
                    Name = account.Name,
                    AccountTypeId = account.AccountTypeId,
                    Balance = account.Balance,
                    Description = account.Description
                });
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

        public async Task<List<AccountModel>> GetAccountsbyUserId(int userId)
        {
            using (IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType))
            {
                try
                {
                    connection.Open();

                    string query = string.Empty;

                    if (_dbType == "SqlServer")
                    {
                        query = @"
                    SELECT Account.Id, Account.Name, Balance, TC.Name as AccountType
                    FROM Account
                    INNER JOIN AccountType TC ON TC.Id = Account.AccountTypeId
                    WHERE TC.UserId = @UserId
                    ORDER BY TC.[Order]";
                    }
                    else if (_dbType == "MySql")
                    {
                        query = @"
                    SELECT Account.Id, Account.Name, Balance, TC.Name as AccountType
                    FROM Account
                    INNER JOIN AccountType TC ON TC.Id = Account.AccountTypeId
                    WHERE TC.UserId = @UserId
                    ORDER BY TC.`Order`";
                    }

                    var accounts = await connection.QueryAsync<AccountModel>(
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
        public async Task<AccountModel> GetAccountById(int id, int userId)
        {
            using (IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType))
            {
                try
                {
                    connection.Open();

                    string query = string.Empty;

                    if (_dbType == "SqlServer")
                    {
                        query = @"
                            SELECT Account.Id, Account.Name, Balance, Account.AccountTypeId , Account.Description
                            FROM Account
                            INNER JOIN AccountType TC ON TC.Id = Account.AccountTypeId
                            WHERE TC.UserId = @UserId And Account.Id = @Id";
                    }
                    else if (_dbType == "MySql")
                    {
                        query = @"
                            SELECT Account.Id, Account.Name, Balance,  Account.AccountTypeId , Account.Description
                            FROM Account
                            INNER JOIN AccountType TC ON TC.Id = Account.AccountTypeId
                            WHERE TC.UserId = @UserId And Account.Id = @Id";
                    }

                    var account = await connection.QueryFirstOrDefaultAsync<AccountModel>(
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

        public async Task UpdateAccount(AccountModelCreation account)
        {
            using (IDbConnection connection = _createConnection.CreateConnectionSql(_connectionString, _dbType))
            {
                try
                {
                    connection.Open();

                    string query = string.Empty;

                    if (_dbType == "SqlServer")
                    {
                        query = @"
                        UPDATE Account 
                        SET Name = @Name, Balance = @Balance, Description = @Description, AccountTypeId = @AccountTypeId
                        WHERE Id = @Id";
                    }
                    else if (_dbType == "MySql")
                    {
                        query = @"
                        UPDATE Account 
                        SET Name = @Name, Balance = @Balance, Description = @Description, AccountTypeId = @AccountTypeId
                        WHERE Id = @Id";
                    }

                   await connection.ExecuteAsync(
                       query, account
                    );

                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }
}
