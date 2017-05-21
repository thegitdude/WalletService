using System.Threading.Tasks;
using WalletService.Model;

namespace WalletService.Service
{
    public interface IAccountService
    {
        Task<int> OpenCustomerAccountAsync(string userId);

        Task<int> CloseCustomerAccountAsync(int accountId);

        Task<Account> GetAccountInformationAsync(int accountId);

        Task<decimal> WithdrawAmountAsync(int accountId, decimal amount);

        Task<decimal> DepositAmountAsync(int accountId, decimal amount);
    }
}