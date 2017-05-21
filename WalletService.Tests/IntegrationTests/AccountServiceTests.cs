using NUnit.Framework;
using Ploeh.AutoFixture;
using System.Threading.Tasks;
using WalletService.Model;
using WalletService.Service;

namespace WalletService.Tests.IntegrationTests
{
    public class AccountServiceTests : IntegrationTestBase
    {
        [Test]
        public async Task CanCreateNewAccount()
        {
            //Arrange
            var userId = Fixture.Create<string>();
            var sut = new AccountService(SqlManager);

            //Act
            var accountId = await sut.OpenCustomerAccountAsync(userId);

            //Assert
            var account = await GetAccountById(accountId);
            Assert.That(account, Is.Not.Null);
            Assert.That(account.UserId, Is.EqualTo(userId));
            Assert.That(account.Active, Is.EqualTo(true));
            Assert.That(account.Balance, Is.EqualTo(0));
        }

        [Test]
        public async Task CanCloseExistingAccount()
        {
            //Arrange
            var userId = Fixture.Create<string>();
            var sut = new AccountService(SqlManager);

            //Act
            var accountId = await sut.OpenCustomerAccountAsync(userId);
            var rowsModified = await sut.CloseCustomerAccountAsync(accountId);

            //Assert
            var account = await GetAccountById(accountId);
            Assert.That(account, Is.Not.Null);
            Assert.That(account.Active, Is.EqualTo(false));
            Assert.That(account.DeletedDate, Is.Not.Null);
        }

        [Test]
        public async Task CanDepositFunds()
        {
            //Arrange
            var userId = Fixture.Create<string>();
            var amount = Fixture.Create<decimal>();
            var sut = new AccountService(SqlManager);

            //Act
            var accountId = await sut.OpenCustomerAccountAsync(userId);
            var rowsModified = await sut.DepositAmountAsync(accountId, amount);

            //Assert
            var account = await GetAccountById(accountId);
            Assert.That(account, Is.Not.Null);
            Assert.That(account.Balance, Is.EqualTo(amount));
        }

        [Test]
        public async Task CanWithdrawAmount()
        {
            //Arrange
            var userId = Fixture.Create<string>();
            var sut = new AccountService(SqlManager);

            //Act
            var accountId = await sut.OpenCustomerAccountAsync(userId);
            await sut.DepositAmountAsync(accountId, 100);
            await sut.WithdrawAmountAsync(accountId, 10);

            //Assert
            var account = await GetAccountById(accountId);
            Assert.That(account, Is.Not.Null);
            Assert.That(account.UserId, Is.EqualTo(userId));
            Assert.That(account.Balance, Is.EqualTo(90));
        }

        [Test]
        public async Task CanGetAccountDetails()
        {
            //Arrange
            var userId = Fixture.Create<string>();
            var sut = new AccountService(SqlManager);

            //Act
            var accountId = await sut.OpenCustomerAccountAsync(userId);
            var account = await sut.GetAccountInformationAsync(accountId);

            //Assert
            Assert.That(account, Is.Not.Null);
            Assert.That(account.UserId, Is.EqualTo(userId));
            Assert.That(account.Active, Is.EqualTo(true));
            Assert.That(account.Balance, Is.EqualTo(0));
        }

        private async Task<Account> GetAccountById(int id)
        {
            const string sql = @"SELECT * FROM Accounts where Id= @Id";

            return await SqlManager.QueryFirstAsync<Account>(sql, new { Id = id});
        }
    }
}
