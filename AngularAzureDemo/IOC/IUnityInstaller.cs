using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Practices.Unity;

namespace AngularAzureDemo.IOC
{
    public interface IUnityInstaller
    {
        void Install(IUnityContainer container);
    }
}