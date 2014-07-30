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
    public class ImageBlobCommentController : ApiController
    {
        private readonly IImageBlobRepository imageBlobRepository;
        private readonly ImageBlobCommentRepository imageBlobCommentRepository;

        public ImageBlobCommentController(IImageBlobRepository imageBlobRepository, ImageBlobCommentRepository imageBlobCommentRepository)
        {
            this.imageBlobRepository = imageBlobRepository;
            this.imageBlobCommentRepository = imageBlobCommentRepository;
        }

        // GET api/imageblobcomment
        [System.Web.Http.HttpGet]
        public async Task<FullImageBlobComments> Get()
        {
            // Return a list of all ImageBlob objects 
            var blobs = await imageBlobRepository.FetchAllBlobs();

            //fetch all comments to form richer results
            var fullImageBlobComments = await FetchBlobComments(blobs);
            return fullImageBlobComments;
           
        }


        // GET api/imageblobcomment/5
        [System.Web.Http.HttpGet]
        public async Task<FullImageBlobComments> Get(int id)
        {
            if (id <= 0)
                return new FullImageBlobComments();

            // Return a list of ImageBlob objects for the selected user
            var blobsForUsers = await imageBlobRepository.FetchBlobsForUser(id);
            //fetch all comments to form richer results
            var fullImageBlobComments = await FetchBlobComments(blobsForUsers);
            return fullImageBlobComments;
           
        }


        private async Task<FullImageBlobComments> FetchBlobComments(IEnumerable<ImageBlob> blobs)
        {
            FullImageBlobComments fullImageBlobComments = new FullImageBlobComments();
            foreach (var blob in blobs)
            {
                var comments = await imageBlobCommentRepository.FetchAllCommentsForBlob(blob.Id);
                fullImageBlobComments.BlobComments.Add(
                    new FullImageBlobComment()
                    {
                        Blob = blob,
                        Comments = comments.ToList()
                    });
            }
            return fullImageBlobComments;
            
        }
    }
}
