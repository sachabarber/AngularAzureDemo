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
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.Queryable;

namespace AngularAzureDemo.DomainServices
{
    public interface IImageBlobCommentRepository
    {
        Task<IEnumerable<ImageBlobComment>> FetchAllCommentsForBlob(Guid associatedBlobId);
    }


    public class ImageBlobCommentRepository : IImageBlobCommentRepository
    {

        private readonly string azureStorageConnectionString;
        private readonly CloudStorageAccount storageAccount;
        private Users users = new Users();
        private const int LIMIT_OF_ITEMS_TO_TAKE = 10;


        public ImageBlobCommentRepository()
        {
            azureStorageConnectionString = ConfigurationManager.AppSettings["azureStorageConnectionString"];
            storageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
        }


        public async Task<IEnumerable<ImageBlobComment>> FetchAllCommentsForBlob(Guid associatedBlobId)
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable imageBlobCommentsTable = tableClient.GetTableReference("ImageBlobComments");

            var tableExists = await imageBlobCommentsTable.ExistsAsync();
            if (!tableExists)
            {
                return new List<ImageBlobComment>();
            }

            //http://blog.liamcavanagh.com/2011/11/how-to-sort-azure-table-store-results-chronologically/
            string rowKeyToUse = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

            List<ImageBlobCommentEntity> blobCommentEntities = new List<ImageBlobCommentEntity>();
            Expression<Func<ImageBlobCommentEntity, bool>> filter = (x) => x.AssociatedBlobId.ToString() == associatedBlobId.ToString() &&
                                                                        x.RowKey.CompareTo(rowKeyToUse) > 0;

            Action<IEnumerable<ImageBlobCommentEntity>> processor = blobCommentEntities.AddRange;
            await this.ObtainBlobCommentEntities(imageBlobCommentsTable, filter, processor);
            var imageBlobs = ProjectToBlobComments(blobCommentEntities);


            return imageBlobs;
        }





        private static List<ImageBlobComment> ProjectToBlobComments(List<ImageBlobCommentEntity> blobCommentEntities)
        {
            var blobComments =
                blobCommentEntities.Select(
                    x =>
                        new ImageBlobComment()
                        {
                            Comment = x.Comment,
                            UserName = x.UserName,
                            CreatedOn = DateTime.Parse(x.CreatedOn),
                            CreatedOnPreFormatted = DateTime.Parse(x.CreatedOn).ToShortDateString()
                        }).ToList();
            return blobComments;
        }


        private async Task<bool> ObtainBlobCommentEntities(
            CloudTable imageBlobsTable,
            Expression<Func<ImageBlobCommentEntity, bool>> filter,
            Action<IEnumerable<ImageBlobCommentEntity>> processor)
        {
            TableQuerySegment<ImageBlobCommentEntity> segment = null;

            while (segment == null || segment.ContinuationToken != null)
            {
                var query = imageBlobsTable
                                .CreateQuery<ImageBlobCommentEntity>()
                                .Where(filter)
                                .Take(LIMIT_OF_ITEMS_TO_TAKE)
                                .AsTableQuery();

                segment = await query.ExecuteSegmentedAsync(segment == null ? null : segment.ContinuationToken);
                processor(segment.Results);
            }

            return true;
        }
    }
}

