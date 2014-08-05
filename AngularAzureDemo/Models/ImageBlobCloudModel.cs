using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Models
{
    public class ImageBlobCloudModel
    {
        public bool TableExists { get; set; }
        public CloudTable Table { get; set; }
    }
}
