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
    /// API controller to manage user subscriptions
    /// </summary>
    public class UserSubscriptionController : ApiController
    {
        private readonly IUserSubscriptionRepository userSubscriptionRepository;

        public UserSubscriptionController(IUserSubscriptionRepository userSubscriptionRepository)
        {
            this.userSubscriptionRepository = userSubscriptionRepository;
        }


        // GET api/usersubscription/5
        [System.Web.Http.HttpGet]
        public async Task<UserSubscriptions> Get(int id)
        {
            
            if (id <= 0)
                return new UserSubscriptions();

            // Return a static list of people
            var subscriptions = await userSubscriptionRepository.FetchSubscriptions(id);
            UserSubscriptions userSubscriptionsToSave = new UserSubscriptions();
            userSubscriptionsToSave.Subscriptions = subscriptions.ToList();
            return userSubscriptionsToSave;
            
        }



        // POST api/usersubscription/....
        [System.Web.Http.HttpPost]
        public async Task<bool> Post(UserSubscriptions userSubscriptions)
        {
       
            var subscriptions = userSubscriptions.Subscriptions;

            if (!subscriptions.Any())
                return false;

            int id = subscriptions[0].UserId;

            if (subscriptions.Any(x => x.UserId != id))
                return false;

            // remove all subscriptions that user chose to remove
            var subscriptionsToDelete = subscriptions.Where(x => !x.IsActive).ToList();
            if (subscriptionsToDelete.Any())
            {
                await userSubscriptionRepository.RemoveSubscriptions(subscriptionsToDelete);
            }

            // add all subscriptions that user now has active
            var subscriptionsToAdd = subscriptions.Where(x => x.IsActive).ToList();
            if (subscriptionsToAdd.Any())
            {
                await userSubscriptionRepository.AddSubscriptions(subscriptionsToAdd);
            }

            return true;

        }


    }
}
