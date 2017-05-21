using System.Data;
using System.Threading.Tasks;

namespace WalletService.Utility
{
    public interface IDbConnectionFactory
    {
        string GetConnectionString();

        Task<IDbConnection> GetConnectionAsync();
    }
}