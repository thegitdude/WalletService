using Autofac;
using System.Configuration;
using WalletService.Utility;

namespace WalletService.AutofacModules
{
    public class SqlModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["sql.connectionString"].ConnectionString;

            builder.Register(c => new DbConnectionFactory(connectionString)).As<IDbConnectionFactory>().InstancePerLifetimeScope();

            builder.RegisterType<SqlManager>().As<ISqlManager>().SingleInstance();
        }
    }
}