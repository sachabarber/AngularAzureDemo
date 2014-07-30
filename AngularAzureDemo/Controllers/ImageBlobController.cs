using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

using AngularAzureDemo.DomainServices;
using AngularAzureDemo.Models;


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
        public async Task<bool> Post(ImageBlobs imageBlobs)
        {
            var blobsToSave = imageBlobs.Blobs;

            if (!blobsToSave.Any() || blobsToSave.Count() > 1)
                return false;

            var blobToSave = imageBlobs.Blobs.First();

            // add the blob to blob storage/table storage
            await imageBlobRepository.AddBlob(blobToSave);

            return true;
            
        }
    }
}
