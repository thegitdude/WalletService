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

[assembly: OwinStartupAttribute(typeof(WalletService.Startup))]
namespace WalletService
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            var builder = new ContainerBuilder();
            // Register dependencies, then...
            var container = builder.Build();

            // Register the Autofac middleware FIRST. This also adds
            // Autofac-injected middleware registered with the container.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            var config = CreateHttpConfiguration();
            
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