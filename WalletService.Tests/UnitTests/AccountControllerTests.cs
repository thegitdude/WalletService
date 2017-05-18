using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Moq;
using WalletService.Service;
using WalletService.Model;
using WalletService.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WalletService.Tests.UnitTests
{
    public class AccountControllerTests : UnitTestBase
    {
        private Mock<IAccountService> _accountServiceMock;

        [SetUp]
        public void SetUp()
        {
            _accountServiceMock = Fixture.Freeze<Mock<IAccountService>>();
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
            var actual = await sut.GetAccountBallance(customerId).ConfigureAwait(false);

            //Assert
            NUnit.Framework.Assert.That(actual, Is.EqualTo(expected.Balance));
        }
    }
}
