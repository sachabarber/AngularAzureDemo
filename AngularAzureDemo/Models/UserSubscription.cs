using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Models
{
    public class UserSubscription
    {
        public int UserId { get; set; }
        public int FriendId { get; set; }
        public bool IsActive { get; set; }
    }
}