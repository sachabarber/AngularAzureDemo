using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Models
{
    public class FullImageBlobComment
    {
        public ImageBlob Blob { get; set; }
        public List<ImageBlobComment> Comments { get; set; }
    }
}