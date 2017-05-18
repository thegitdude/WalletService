using System.Collections.Generic;
using System.Threading.Tasks;

namespace WalletService.Utility
{
    public interface ISqlManager
    {
        Task<int> ExecuteAsync(string sql, object param);

        Task<IEnumerable<T>> QueryAsync<T>(string sql, object param);

        Task<T> QueryFirstAsync<T>(string sql, object param);
    }
}