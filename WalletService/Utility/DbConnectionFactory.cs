using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WalletService.Utility
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> GetConnectionAsync()
        {
            var dbConnection = new SqlConnection(_connectionString);
            await dbConnection.OpenAsync();

            return dbConnection;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}