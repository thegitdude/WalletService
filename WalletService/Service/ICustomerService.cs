using System.Threading.Tasks;
using WalletService.Model;

namespace WalletService.Service
{
    public interface ICustomerService
    {
        Task<int> OpenCustomerAccountAsync(int customerId);

        Task<int> CloseCustomerAccountAsync(int customerId);

        Task<Account> GetAccountInformation(int customerId);

        Task<decimal> WithdrawAmount(int customerId, decimal amountRequested);
    } 
}