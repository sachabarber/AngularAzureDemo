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
    public interface IImageBlobRepository
    {
        Task<IEnumerable<ImageBlob>> FetchAllBlobs();
        Task<IEnumerable<ImageBlob>> FetchBlobsForUser(int userId);
        Task<bool> AddBlob(ImageBlob imageBlobToStore);
    }


    public class ImageBlobRepository : IImageBlobRepository
    {

        private readonly string azureStorageConnectionString;
        private readonly CloudStorageAccount storageAccount;
        private Users users = new Users();
        private const int LIMIT_OF_ITEMS_TO_TAKE  = 10;


        public ImageBlobRepository()
        {
            azureStorageConnectionString = ConfigurationManager.AppSettings["azureStorageConnectionString"];
            storageAccount = CloudStorageAccount.Parse(azureStorageConnectionString);
        }

        public async Task<IEnumerable<ImageBlob>> FetchAllBlobs()
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable imageBlobsTable = tableClient.GetTableReference("ImageBlobs");

            var tableExists = await imageBlobsTable.ExistsAsync();
            if (!tableExists)
            {
                return new List<ImageBlob>();
            }

            List<ImageBlob> imageBlobs = new List<ImageBlob>();

            //http://blog.liamcavanagh.com/2011/11/how-to-sort-azure-table-store-results-chronologically/
            string rowKeyToUse = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

            foreach (var user in users)
            {
                List<ImageBlobEntity> imageBlobEntities = new List<ImageBlobEntity>();
                Expression<Func<ImageBlobEntity, bool>> filter = (x) => x.PartitionKey == user.Id.ToString() &&
                                                                        x.RowKey.CompareTo(rowKeyToUse) > 0;

                Action<IEnumerable<ImageBlobEntity>> processor = imageBlobEntities.AddRange;
                await ObtainImageBlobEntities(imageBlobsTable, filter, processor);
                var projectedImages = ProjectToImageBlobs(imageBlobEntities);
                imageBlobs.AddRange(projectedImages);

            }
            return imageBlobs;
        }

        public async Task<IEnumerable<ImageBlob>> FetchBlobsForUser(int userId)
        {
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable imageBlobsTable = tableClient.GetTableReference("ImageBlobs");

            var tableExists = await imageBlobsTable.ExistsAsync();
            if (!tableExists)
            {
                return new List<ImageBlob>();
            }

            //http://blog.liamcavanagh.com/2011/11/how-to-sort-azure-table-store-results-chronologically/
            string rowKeyToUse = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);

            List<ImageBlobEntity> imageBlobEntities = new List<ImageBlobEntity>();
            Expression<Func<ImageBlobEntity, bool>> filter = (x) => x.PartitionKey == userId.ToString() &&
                                                                        x.RowKey.CompareTo(rowKeyToUse) > 0;

            Action<IEnumerable<ImageBlobEntity>> processor = imageBlobEntities.AddRange;
            await ObtainImageBlobEntities(imageBlobsTable, filter, processor);
            var imageBlobs = ProjectToImageBlobs(imageBlobEntities);


            return imageBlobs;
        }

       

        public async Task<bool> AddBlob(ImageBlob imageBlobToStore)
        {
            BlobStorageResult blobStorageResult = await StoreImageInBlobStorage(imageBlobToStore);
            if (!blobStorageResult.StoredOk)
            {
                return false;
            }
            else
            {
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
                CloudTable imageBlobsTable = tableClient.GetTableReference("ImageBlobs");

                var tableExists = await imageBlobsTable.ExistsAsync();
                if (!tableExists)
                {
                    await imageBlobsTable.CreateIfNotExistsAsync();
                }
                
                ImageBlobEntity imageBlobEntity = new ImageBlobEntity(
                        imageBlobToStore.UserId,
                        imageBlobToStore.UserName,
                        Guid.NewGuid(),
                        blobStorageResult.BlobUrl,
                        imageBlobToStore.Title,
                        imageBlobToStore.CreatedOn.ToShortDateString()
                    );


                TableOperation insertOperation = TableOperation.Insert(imageBlobEntity);
                imageBlobsTable.Execute(insertOperation);
               
                return true;
            }
  

        }

        private static List<ImageBlob> ProjectToImageBlobs(List<ImageBlobEntity> imageBlobEntities)
        {
            var imageBlobs =
                imageBlobEntities.Select(
                    x =>
                        new ImageBlob()
                        {
                            UserId = int.Parse(x.PartitionKey),
                            UserName = x.UserName,
                            SavedBlobUrl = x.BlobUrl,
                            Id = x.Id,
                            Title = x.Title,
                            CreatedOn = DateTime.Parse(x.CreatedOn)
                        }).ToList();
            return imageBlobs;
        }

        private async Task<BlobStorageResult> StoreImageInBlobStorage(ImageBlob imageBlobToStore)
        {
           
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("images");

            bool created = container.CreateIfNotExists();
            container.SetPermissionsAsync(
                new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });


            var blockBlob = container.GetBlockBlobReference(string.Format(@"{0}.image/png", 
                Guid.NewGuid().ToString()));
            string marker = "data:image/png;base64,";
            string dataWithoutJpegMarker = imageBlobToStore.CanvasData.Replace(marker, String.Empty);
            byte[] filebytes = Convert.FromBase64String(dataWithoutJpegMarker);

            blockBlob.UploadFromByteArray(filebytes, 0, filebytes.Length);
            return new BlobStorageResult(true, blockBlob.Uri.ToString());
        }

        private async Task<bool> ObtainImageBlobEntities(
            CloudTable imageBlobsTable,
            Expression<Func<ImageBlobEntity, bool>> filter,
            Action<IEnumerable<ImageBlobEntity>> processor)
        {
            TableQuerySegment<ImageBlobEntity> segment = null;

            while (segment == null || segment.ContinuationToken != null)
            {
                var query = imageBlobsTable
                                .CreateQuery<ImageBlobEntity>()
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

