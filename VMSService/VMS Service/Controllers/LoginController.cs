using System.Web.Http;
using VMS_Service.Models;

namespace VMS_Service.Controllers
{
    public class LoginController : ApiController
    {
        [HttpGet]
        public User Login(string username, string password)
        {
            return new Models.User();
        }
    }
}
