using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AngularAzureDemo.DomainServices;

using Microsoft.Practices.Unity;

namespace AngularAzureDemo.IOC
{
    public static class UnityContainerExtensions
    {
        public static void Install(this IUnityContainer container, IUnityInstaller installer)
        {
            installer.Install(container);
        }
    }
}