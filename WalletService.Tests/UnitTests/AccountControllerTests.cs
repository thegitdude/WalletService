using NUnit.Framework;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Moq;
using WalletService.Service;
using WalletService.Model;
using WalletService.Controllers;
using System.Web.Http.Results;
using WalletService.Exceptions;
using System;
using System.Net;

namespace WalletService.Tests.UnitTests
{
    public class AccountControllerTests : UnitTestBase
    {
        private Mock<IAccountService> _accountServiceMock;

        [SetUp]
        public void SetUpTest()
        {
            _accountServiceMock = Fixture.Freeze<Mock<IAccountService>>();
        }

        //For the CeateAccountAsync endpoint I created a test for each returned status code (Ok, BadRequest, InternalServerError)
        //Tests in the same style should be added for the other endpoints but for example purposes this should be enough

        [Test]
        public async Task CanCreateAccount()
        {
            //Arrange
            var userId = Fixture.Create<string>();
            var accountId = Fixture.Create<int>();
            _accountServiceMock.Setup(x => x.OpenCustomerAccountAsync(userId)).ReturnsAsync(accountId);

            var sut = new AccountController(_accountServiceMock.Object);

            //Act
            var actual = await sut.CreateAccountAsync(userId).ConfigureAwait(false) as OkNegotiatedContentResult<int>;

            //Assert
            Assert.That(actual.Content, Is.EqualTo(accountId));
        }

        [Test]
        public async Task CreateAccountReturnsBadRequestWhenUserIDNotUnique()
        {
            //Arrange
            var userId = Fixture.Create<string>();
            var accountId = Fixture.Create<int>();
            _accountServiceMock.Setup(x => x.OpenCustomerAccountAsync(userId))
                .ThrowsAsync(Fixture.Create<UserIdNotUniqueException>());

            var sut = new AccountController(_accountServiceMock.Object);

            //Act
            var actual = await sut.CreateAccountAsync(userId).ConfigureAwait(false) as BadRequestErrorMessageResult;

            //Assert
            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public async Task CreateAccountReturnsInternalServerErrorForUnexpectedExceptions()
        {
            //Arrange
            var userId = Fixture.Create<string>();
            var accountId = Fixture.Create<int>();
            _accountServiceMock.Setup(x => x.OpenCustomerAccountAsync(userId))
                .ThrowsAsync(Fixture.Create<Exception>());

            var sut = new AccountController(_accountServiceMock.Object);

            //Act
            var actual = await sut.CreateAccountAsync(userId).ConfigureAwait(false) as NegotiatedContentResult<string>;

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task CanGetAccountBallance()
        {
            //Arrange
            var customerId = Fixture.Create<int>();
            var expected = Fixture.Create<Account>();
            _accountServiceMock.Setup(x => x.GetAccountInformationAsync(customerId)).ReturnsAsync(expected);

            var sut = new AccountController(_accountServiceMock.Object);

            //Act
            var actual = await sut.GetAccountBallance(customerId).ConfigureAwait(false) as OkNegotiatedContentResult<decimal>;

            //Assert
            Assert.That(actual.Content, Is.EqualTo(expected.Balance));
        }

        [Test]
        public async Task GetAccountBlanceReturnsBadRequestWhenAccountNotFound()
        {
            //Arrange
            var accountId = Fixture.Create<int>();
            _accountServiceMock.Setup(x => x.GetAccountInformationAsync(accountId))
                .ThrowsAsync(Fixture.Create<AccountNotFoundException>());

            var sut = new AccountController(_accountServiceMock.Object);

            //Act
            var actual = await sut.GetAccountBallance(accountId).ConfigureAwait(false) as NegotiatedContentResult<string>;

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task GetAccountBlanceReturnsInternalServerErrorForUnexpectedExceptions()
        {
            //Arrange
            var accountId = Fixture.Create<int>();
            _accountServiceMock.Setup(x => x.GetAccountInformationAsync(accountId))
                .ThrowsAsync(Fixture.Create<Exception>());

            var sut = new AccountController(_accountServiceMock.Object);

            //Act
            var actual = await sut.GetAccountBallance(accountId).ConfigureAwait(false) as NegotiatedContentResult<string>;

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public async Task CanCloseAccount()
        {
            //Arrange
            var accountId = Fixture.Create<int>();
            _accountServiceMock.Setup(x => x.CloseCustomerAccountAsync(accountId))
                .ReturnsAsync(1);

            var sut = new AccountController(_accountServiceMock.Object);

            //Act
            var actual = await sut.CloseAccountAsync(accountId).ConfigureAwait(false) as OkResult;

            //Assert
            Assert.That(actual, Is.Not.Null);
        }

        [Test]
        public async Task CloseAccountReturnsBadRequestWhenAccountNotFound()
        {
            //Arrange
            var accountId = Fixture.Create<int>();
            _accountServiceMock.Setup(x => x.CloseCustomerAccountAsync(accountId))
                .ThrowsAsync(Fixture.Create<AccountNotFoundException>());

            var sut = new AccountController(_accountServiceMock.Object);

            //Act
            var actual = await sut.CloseAccountAsync(accountId).ConfigureAwait(false) as NegotiatedContentResult<string>;

            //Assert
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public async Task CloseAccountReturnsBadRequestWhenAccountCloseException()
        {
            //Arrange
            var accountId = Fixture.Create<int>();
            _accountServiceMock.Setup(x => x.CloseCustomerAccountAsync(accountId))
                .ThrowsAsync(Fixture.Create<AccountCloseException>());

            var sut = new AccountController(_accountServiceMock.Object);

            //Act
            var actual = await sut.CloseAccountAsync(accountId).ConfigureAwait(false) as BadRequestErrorMessageResult;

            //Assert
            Assert.That(actual, Is.Not.Null);
        }
    }
}
