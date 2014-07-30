using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.WindowsAzure.Storage.Table;

namespace AngularAzureDemo.Models
{
    public class FullImageBlobComments
    {
        public FullImageBlobComments()
        {
            BlobComments = new List<FullImageBlobComment>();
        }

        public IList<FullImageBlobComment> BlobComments { get; set; }
    }
}