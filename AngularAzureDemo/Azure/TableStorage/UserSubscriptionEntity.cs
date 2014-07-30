using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Azure.TableStorage
{
    public class UserSubscriptionEntity : TableEntity
    {
        public UserSubscriptionEntity(int userId, int friendId)
        {
            this.PartitionKey = userId.ToString();
            this.RowKey = friendId.ToString();
        }

        public UserSubscriptionEntity() { }
    }
}