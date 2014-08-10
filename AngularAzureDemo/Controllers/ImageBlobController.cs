using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

using AngularAzureDemo.DomainServices;
using AngularAzureDemo.Models;
using AngularAzureDemo.SignalR;
using Microsoft.AspNet.SignalR;


namespace AngularAzureDemo.Controllers
{
    /// <summary>
    /// API controller to store user images and their metadata
    /// </summary>
    public class ImageBlobController : ApiController
    {
        private readonly IImageBlobRepository imageBlobRepository;

        public ImageBlobController(IImageBlobRepository imageBlobRepository)
        {
            this.imageBlobRepository = imageBlobRepository;
        }

        // POST api/imageblob/....
        [System.Web.Http.HttpPost]
        public async Task<bool> Post(ImageBlob imageBlob)
        {
            if (imageBlob == null || imageBlob.CanvasData == null)
                return false;

            // add the blob to blob storage/table storage
            var storedImageBlob = await imageBlobRepository.AddBlob(imageBlob);
            if (storedImageBlob != null)
            {
                BlobHub.SendFromWebApi(storedImageBlob);
            }
            return false;
        }
    }
}
