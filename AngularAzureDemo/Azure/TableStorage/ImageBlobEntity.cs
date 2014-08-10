using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Azure.TableStorage
{
    public class ImageBlobEntity : TableEntity
    {
        public ImageBlobEntity(int userId, string userName, Guid id, string blobUrl, string title, DateTime createdOn)
        {
            this.PartitionKey = userId.ToString();
            this.RowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
            this.Title = title;
            this.Id = id;
            this.UserName = userName;
            this.BlobUrl = blobUrl;
            this.CreatedOn = createdOn;
        }

        public ImageBlobEntity() { }


        public string Title { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string BlobUrl { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}