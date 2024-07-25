using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using System.Data;

namespace MoneyBudgets.Extensions;

public class CreateConnection
{
    public IDbConnection CreateConnectionSql(string connectionString, string dbType)
    {
        if (dbType == "SqlServer")
        {
            return new SqlConnection(connectionString);
        }
        else if (dbType == "MySql")
        {
            return new MySqlConnection(connectionString);
        }
        else
        {
            throw new Exception("Unsupported database type");
        }
    }

}