using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AngularAzureDemo.SignalRStartup))]

namespace AngularAzureDemo
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
} 