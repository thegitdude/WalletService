using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Integration.WebApi;
using Microsoft.Owin;
using Owin;
using System.Reflection;
using System.Web.Http;
using Microsoft.Owin.Host;
using WalletService.Service;
using WalletService.Utility;

[assembly: OwinStartupAttribute(typeof(WalletService.Startup))]
namespace WalletService
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            // Register dependencies, then...

            // Register the Autofac middleware FIRST. This also adds
            // Autofac-injected middleware registered with the container.
            builder.RegisterType<SqlManager>().As<ISqlManager>().SingleInstance();
            builder.RegisterType<AccountService>().As<IAccountService>().SingleInstance();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var config = CreateHttpConfiguration();

            var container = builder.Build();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);

            app.UseWebApi(config);
        }

        public static HttpConfiguration CreateHttpConfiguration()
        {
            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();

            return httpConfiguration;
        }
    }
}