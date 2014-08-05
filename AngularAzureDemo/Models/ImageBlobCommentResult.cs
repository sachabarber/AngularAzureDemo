using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AngularAzureDemo.Models
{
    public class ImageBlobCommentResult
    {
        public bool SuccessfulAdd { get; set; }
        public ImageBlobComment Comment { get; set; }
    }
}