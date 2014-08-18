using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using AngularAzureDemo.DomainServices;
using AngularAzureDemo.IOC;

using Microsoft.Practices.Unity;

namespace AngularAzureDemo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //IOC
            var container = new UnityContainer();
            container.Install(new WebApiInstaller());


            //set IOC resolver
            GlobalConfiguration.Configuration.DependencyResolver = new UnityResolver(container);
        }
    }
}