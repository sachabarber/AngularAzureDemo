using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AngularAzureDemo.Models;

namespace AngularAzureDemo.Controllers
{
    /// <summary>
    /// API controller to manage users
    /// </summary>
    public class UserController : ApiController
    {
        private readonly Users users;

        public UserController()
        {
            users = new Users();
        }

        // GET api/user
        public IEnumerable<User> Get()
        {
            // Return a static list of people
            return users;
        }

        // GET api/user/5
        [System.Web.Http.HttpGet]
        public IEnumerable<User> Get(int id)
        {
            //return static list of people which do not include the current person
            return users.Where(x => x.Id != id);
        }
    }
}
