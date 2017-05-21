using NUnit.Framework;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using WalletService.Migration;
using WalletService.Tests.UnitTests;
using WalletService.Utility;

namespace WalletService.Tests.IntegrationTests
{
    public class IntegrationTestBase : UnitTestBase
    {
        private IDbConnectionFactory _dbConnectionFactory;
        internal ISqlManager SqlManager;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _dbConnectionFactory = await GetTestDbConnectionFactory();
            Program.Main(new[] { _dbConnectionFactory.GetConnectionString() });

            SqlManager = new SqlManager(_dbConnectionFactory);
        }

        private async Task<IDbConnectionFactory> GetTestDbConnectionFactory()
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["sql.connectionString"].ConnectionString;
            var testDbName = "Test_" + DateTime.Now.ToString("yyyyMMddHHmmss");

            try
            {
                await CreateTestDb(connectionString, testDbName);

                SqlConnectionStringBuilder conn = new SqlConnectionStringBuilder(connectionString)
                { ConnectTimeout = 5, InitialCatalog = testDbName };

                return new DbConnectionFactory(conn.ConnectionString);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async Task CreateTestDb(string sqlConnectionString, string testDbName)
        {
            var sql = "CREATE DATABASE " + testDbName + " ; ";

            using (var db = new SqlConnection(sqlConnectionString))
            {
                await db.OpenAsync();
                SqlCommand command = new SqlCommand(sql, db);
                var result = await command.ExecuteNonQueryAsync();
            }
        }

        [OneTimeTearDown]
        public async Task TearDown()
        {
            await DropTestDb();
        }

        private async Task DropTestDb()
        {
            var sqlString = new SqlConnectionStringBuilder(_dbConnectionFactory.GetConnectionString());

            var sql = $"Use master ALTER DATABASE [{sqlString.InitialCatalog}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE DROP DATABASE [{sqlString.InitialCatalog}]";

            try {
                var result = await SqlManager.ExecuteAsync(sql, new {});
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
