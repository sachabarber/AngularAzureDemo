using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

using AngularAzureDemo.Azure.TableStorage;
using AngularAzureDemo.Models;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;

namespace AngularAzureDemo.DomainServices
{
    public interface IUserSubscriptionRepository
    {
        Task<bool> AddSubscriptions(IEnumerable<UserSubscription> subscriptionsToAdd);
        Task<IEnumerable<UserSubscription>> FetchSubscriptions(int userId);
        Task<bool> RemoveSubscriptions(IEnumerable<UserSubscription> subscriptionsToRemove);
    }


    public class UserSubscriptionRepository : IUserSubscriptionRepository
    {

        private readonly string azureStorageConnectionString;
        private readonly CloudStorageAccount storageAccount;

        public UserSubscriptionRepository()
        {
            azureStorageConnectionString = ConfigurationManager.AppSettings["azureStorageConnectionString"];
            storageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
        }



        public async Task<bool> AddSubscriptions(IEnumerable<UserSubscription> subscriptionsToAdd)
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable userSubscriptionsTable = tableClient.GetTableReference("userSubscriptions");

            var tableExists = await userSubscriptionsTable.ExistsAsync();
            if (!tableExists)
            {
                await userSubscriptionsTable.CreateIfNotExistsAsync();
            }

            TableBatchOperation batchOperation = new TableBatchOperation();
            foreach (var subscription in subscriptionsToAdd)
            {
                UserSubscriptionEntity userSubscriptionEntity =
                    new UserSubscriptionEntity(subscription.UserId, subscription.FriendId);
                batchOperation.InsertOrReplace(userSubscriptionEntity);
            }
            await userSubscriptionsTable.ExecuteBatchAsync(batchOperation);

            return true;
        }

        public async Task<IEnumerable<UserSubscription>> FetchSubscriptions(int userId)
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable userSubscriptionsTable = tableClient.GetTableReference("userSubscriptions");

            var tableExists = await userSubscriptionsTable.ExistsAsync();
            if (!tableExists)
            {
                return new List<UserSubscription>();
            }

            List<UserSubscriptionEntity> activeUserSubscriptionEntities = new List<UserSubscriptionEntity>();
            Expression<Func<UserSubscriptionEntity, bool>> filter = (x) => x.PartitionKey == userId.ToString();
            
            Action<IEnumerable<UserSubscriptionEntity>> processor = activeUserSubscriptionEntities.AddRange;

            await ObtainUserSubscriptionEntities(userSubscriptionsTable, filter, processor);

            var userSubscriptions = activeUserSubscriptionEntities.Select(x => new UserSubscription()
            {
                UserId = int.Parse(x.PartitionKey),
                FriendId = int.Parse(x.RowKey),
                IsActive = true
            }).ToList();
          

            return userSubscriptions;
        }

        public async Task<bool> RemoveSubscriptions(IEnumerable<UserSubscription> subscriptionsToRemove)
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable userSubscriptionsTable = tableClient.GetTableReference("userSubscriptions");

            var tableExists = await userSubscriptionsTable.ExistsAsync();
            if (!tableExists)
            {
                return false;
            }

            List<UserSubscriptionEntity> activeUserSubscriptionEntities = new List<UserSubscriptionEntity>();
            Expression<Func<UserSubscriptionEntity, bool>> filter = (x) => x.PartitionKey == subscriptionsToRemove.First().UserId.ToString();

            Action<IEnumerable<UserSubscriptionEntity>> processor = activeUserSubscriptionEntities.AddRange;

            await ObtainUserSubscriptionEntities(userSubscriptionsTable, filter, processor);

            TableBatchOperation deletionBatchOperation = new TableBatchOperation();
            foreach (var userSubscription in subscriptionsToRemove)
            {
                var entity = activeUserSubscriptionEntities.SingleOrDefault(
                    x => x.PartitionKey == userSubscription.UserId.ToString() && 
                    x.RowKey == userSubscription.FriendId.ToString());

                if (entity != null)
                {
                    deletionBatchOperation.Add(TableOperation.Delete(entity));
                }
            }

            if (deletionBatchOperation.Any())
            {
                await userSubscriptionsTable.ExecuteBatchAsync(deletionBatchOperation);
            }
            return true;


        }


        private async Task<bool> ObtainUserSubscriptionEntities(
            CloudTable userSubscriptionsTable,
            Expression<Func<UserSubscriptionEntity, bool>> filter,
            Action<IEnumerable<UserSubscriptionEntity>> processor)
        {
            TableQuerySegment<UserSubscriptionEntity> segment = null;

            while (segment == null || segment.ContinuationToken != null)
            {
                var query = userSubscriptionsTable
                                .CreateQuery<UserSubscriptionEntity>()
                                .Where(filter)
                                .AsTableQuery();

                segment = await query.ExecuteSegmentedAsync(segment == null ? null : segment.ContinuationToken);
                processor(segment.Results);
            }

            return true;
        }

    }
}

