using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularAzureDemo.Models
{
    public class BlobStorageResult
    {
        public BlobStorageResult(bool storedOk, string blobUrl)
        {
            this.StoredOk = storedOk;
            this.BlobUrl = blobUrl;
        }

        public bool StoredOk { get; private set; }
        public string BlobUrl { get; private set; }
    }
}