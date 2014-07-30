using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Azure.TableStorage
{
    public class ImageBlobCommentEntity : TableEntity
    {
        public ImageBlobCommentEntity(int userId, string userName, Guid id, Guid associatedBlobId, string comment, string createdOn)
        {
            this.PartitionKey = userId.ToString();
            this.RowKey = string.Format("{0:D19}", DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks);
            this.Comment = comment;
            this.Id = id;
            this.UserName = userName;
            this.AssociatedBlobId = associatedBlobId;
            this.CreatedOn = createdOn;
        }

        public ImageBlobCommentEntity() { }


        public string Comment { get; set; }
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Guid AssociatedBlobId { get; set; }
        public string CreatedOn { get; set; }

    }
}