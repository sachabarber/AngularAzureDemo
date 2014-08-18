using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using AngularAzureDemo.DomainServices;

using Microsoft.Practices.Unity;

namespace AngularAzureDemo.IOC
{
    public class WebApiInstaller : IUnityInstaller
    {
        public void Install(IUnityContainer container)
        {
            container.RegisterType<IUserSubscriptionRepository, UserSubscriptionRepository>(
                new HierarchicalLifetimeManager());
            container.RegisterType<IImageBlobRepository, ImageBlobRepository>(
                new HierarchicalLifetimeManager());
            container.RegisterType<IImageBlobCommentRepository, ImageBlobCommentRepository>(
                new HierarchicalLifetimeManager());
        }
    }
}