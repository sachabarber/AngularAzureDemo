using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Models
{
    public class ImageBlob
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public Guid Id { get; set; }
        public string SavedBlobUrl { get; set; }
        public string CanvasData { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}