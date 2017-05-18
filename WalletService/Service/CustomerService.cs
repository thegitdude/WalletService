using System;
using System.Threading.Tasks;
using WalletService.Model;
using WalletService.Utility;

namespace WalletService.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ISqlManager _sqlManager;

        public CustomerService(ISqlManager sqlManager)
        {
            _sqlManager = sqlManager;
        }

        public async Task<int> CloseCustomerAccountAsync(int customerId)
        {
            const string sql = "UDATE Accounts SET Deleted = 1, DeletedDate = GETDATE() WHERE customerId = @Id";

            return await _sqlManager.ExecuteAsync(sql, new { Id = customerId }).ConfigureAwait(false);
        }

        public async Task<Account> GetAccountInformation(int customerId)
        {
            const string sql = "SELECT * from Accounts WHERE customerId = @Id";

            return await _sqlManager.QueryFirstAsync<Account>(sql, new { Id = customerId }).ConfigureAwait(false);
        }

        public Task<int> OpenCustomerAccountAsync(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> WithdrawAmount(int customerId, decimal amountRequested)
        {
            throw new NotImplementedException();
        }
    }
}