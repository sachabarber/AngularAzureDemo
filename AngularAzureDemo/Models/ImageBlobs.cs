using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Models
{
    public class ImageBlobs
    {
        public ImageBlobs()
        {
            Blobs = new List<ImageBlob>();
        }

        public IList<ImageBlob> Blobs { get; set; }
    }
}