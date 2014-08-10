using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Web;
using AngularAzureDemo.Models;
using Microsoft.AspNet.SignalR;

namespace AngularAzureDemo.SignalR
{
    public class BlobHub : Hub
    {
        public void Send(ImageBlob latestBlob)
        {
            Clients.All.latestBlobMessage(latestBlob);
        }

        //Called from Web Api controller, so must use GlobalHost context resolution
        public static void SendFromWebApi(ImageBlob imageBlob)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<BlobHub>();
            hubContext.Clients.All.latestBlobMessage(imageBlob);
        }
    }
}