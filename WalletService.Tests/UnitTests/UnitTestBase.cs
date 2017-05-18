using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;

namespace WalletService.Tests.UnitTests
{
    public class UnitTestBase
    {
        internal IFixture Fixture;

        [OneTimeSetUp]
        public void SetUp()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
    }
}
