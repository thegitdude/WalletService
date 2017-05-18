using System;
using System.Threading.Tasks;
using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace WalletService.Utility
{
    public class SqlManager : ISqlManager
    {
        private static readonly string sqlConnectionString = ConfigurationManager.ConnectionStrings["sql.connectionString"].ConnectionString;

        public async Task<int> ExecuteAsync(string sql, object param)
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionString))
            {
                return await db.ExecuteAsync(sql, param).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param)
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionString))
            {
                return await db.QueryAsync<T>(sql, param).ConfigureAwait(false);
            }
        }

        public async Task<T> QueryFirstAsync<T>(string sql, object param)
        {
            using (IDbConnection db = new SqlConnection(sqlConnectionString))
            {
                return await db.QueryFirstAsync<T>(sql, param).ConfigureAwait(false);
            }
        }
    }
}