using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Models
{
    public class ImageBlobComment
    {
        public string Comment { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedOnPreFormatted { get; set; }
        public int UserId { get; set; }
        public Guid Id { get; set; }
        public Guid AssociatedBlobId { get; set; }
    }
}