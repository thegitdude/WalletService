using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WalletService.Exceptions;
using WalletService.Model;
using WalletService.Utility;

namespace WalletService.Service
{
    public class AccountService : IAccountService
    {
        private readonly ISqlManager _sqlManager;

        private const int minimumAllowedBalance = 0;

        public AccountService(ISqlManager sqlManager)
        {
            _sqlManager = sqlManager;
        }

        public async Task<int> CloseCustomerAccountAsync(int accountId)
        {
            const string sql = @"UPDATE Accounts SET Active = 0, DeletedDate = GETDATE(), ModifiedDate = GETDATE() 
                                 WHERE Id = @Id";

            var affectedRows = await _sqlManager.ExecuteAsync(sql, new { Id = accountId }).ConfigureAwait(false);

            if (affectedRows == 0)
                throw new AccountNotFoundException($"Could not find account with id: {accountId}");

            return affectedRows;
        }

        public async Task<Account> GetAccountInformationAsync(int accountId)
        {
            const string sql = @"SELECT Id, UserId, CreatedDate, ModifiedDate, DeletedDate, Balance, Active
                                 FROM Accounts WHERE Id = @Id";

            var accountInformation = await _sqlManager.QueryFirstAsync<Account>(sql, new { Id = accountId }).ConfigureAwait(false);

            if (accountInformation == null)
                throw new AccountNotFoundException($"Could not find account with id: {accountId}");

            return accountInformation;
        }

        public async Task<int> OpenCustomerAccountAsync(string userId)
        {
            const string sql = @"INSERT INTO Accounts
                                 (UserId, CreatedDate, ModifiedDate, Balance)
                                 VALUES
                                 (@UserId, GETDATE(), GETDATE(), 0)
                                 SELECT SCOPE_IDENTITY();";
            try
            {
                return await _sqlManager.QueryFirstAsync<int>(sql, new { UserId = userId }).ConfigureAwait(false);
            }
            catch (SqlException e)
            {
                if (e.Number == 2601)
                    throw new UserIdNotUniqueException($"Coult not create account. An account with the userId {userId} already exists!");

                throw;
            }
        }

        public async Task<int> UpdateBalanceAsync(int accountId, decimal newBalance)
        {
            const string sql = "UPDATE Accounts SET Balance = @Balance, ModifiedDate = GETDATE() WHERE Id = @Id";

            return await _sqlManager.ExecuteAsync(sql, new { Id = accountId, Balance = newBalance }).ConfigureAwait(false);
        }

        public async Task<decimal> WithdrawAmountAsync(int accountId, decimal amount)
        {
            var account = await GetAccountInformationAsync(accountId).ConfigureAwait(false);

            var tempBalance = account.Balance - amount;

            if (tempBalance < minimumAllowedBalance)
                throw new NotEnoughFundsException($"Requested withdraw was larger than the available balance of: {account.Balance}");

            await UpdateBalanceAsync(accountId, tempBalance).ConfigureAwait(false);

            return tempBalance;
        }

        public async Task<decimal> DepositAmountAsync(int accountId, decimal amount)
        {
            const string sql = @"UPDATE Accounts SET Balance = Balance + @Amount, ModifiedDate = GETDATE() 
                                OUTPUT INSERTED.Balance
                                WHERE id= @Id ";

            var newBalance = await _sqlManager.QueryFirstAsync<decimal?>(sql, new { Id = accountId, Amount = amount }).ConfigureAwait(false);

            if (newBalance == null)
                throw new AccountNotFoundException($"Could not find account with id: {accountId}");

            return (decimal)newBalance;
        }
    }
}