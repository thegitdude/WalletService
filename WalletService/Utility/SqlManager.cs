using System.Threading.Tasks;
using Dapper;
using System.Collections.Generic;

namespace WalletService.Utility
{
    public class SqlManager : ISqlManager
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public SqlManager(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<int> ExecuteAsync(string sql, object param)
        {
            using (var dbConnection = await _dbConnectionFactory.GetConnectionAsync())
            {
                return await dbConnection.ExecuteAsync(sql, param).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param)
        {
            using (var dbConnection = await _dbConnectionFactory.GetConnectionAsync())
            {
                return await dbConnection.QueryAsync<T>(sql, param).ConfigureAwait(false);
            }
        }

        public async Task<T> QueryFirstAsync<T>(string sql, object param)
        {
            using (var dbConnection = await _dbConnectionFactory.GetConnectionAsync())
            {
                return await dbConnection.QueryFirstOrDefaultAsync<T>(sql, param).ConfigureAwait(false);
            }
        }
    }
}