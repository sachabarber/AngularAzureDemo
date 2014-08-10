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


        // GET api/imageblobcomment/4E89064B-D1B1-471C-8B2F-C02B374A9676
        [System.Web.Http.HttpGet]
        public async Task<FullImageBlobComment> Get(Guid id)
        {
            if (Guid.Empty == id)
                return new FullImageBlobComment();

            // Return a blob that matched the Id requested
            var blob = await imageBlobRepository.FetchBlobForBlobId(id);
            //fetch all comments to form richer results
            var fullImageBlobComments = await FetchBlobComments(new List<ImageBlob>() {
                blob.First()
            });

            return fullImageBlobComments.BlobComments.Any() ? 
                fullImageBlobComments.BlobComments.First() : new FullImageBlobComment();
        }


        // POST api/imageblobcomment/....
        [System.Web.Http.HttpPost]
        public async Task<ImageBlobCommentResult> Post(ImageBlobComment imageBlobCommentToSave)
        {
            if (imageBlobCommentToSave == null)
                return new ImageBlobCommentResult() { Comment = null, SuccessfulAdd = false};

            if (string.IsNullOrEmpty(imageBlobCommentToSave.Comment))
                return new ImageBlobCommentResult() { Comment = null, SuccessfulAdd = false };

            // add the imageBlobComment to imageBlobComment storage/table storage
            var insertedComment = await imageBlobCommentRepository.AddImageBlobComment(imageBlobCommentToSave);

            return new ImageBlobCommentResult() { Comment = insertedComment, SuccessfulAdd = true };
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
