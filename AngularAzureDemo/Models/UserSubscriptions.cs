using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Models
{
    public class UserSubscriptions
    {

        public UserSubscriptions()
        {
            Subscriptions = new List<UserSubscription>();
        }

        public IList<UserSubscription> Subscriptions { get; set; }
    }
}