namespace VMS_Service.Controllers
{
    using System.Web.Http;
    using VMS_Service.Database;
    using VMS_Service.Models;

    public class LoginController : ApiController
    {
        private IVMSStore _store;
        public LoginController()
        {
            _store = new VMSStore();
        }
        [HttpGet]
        public User Login(string email, string password)
        {
            var user = _store.GetOrganizer(email);
            if(user == null)
            return new Models.User
            {
                IsAuthenticated = false
            };

            return new User
            {
                UserName = user.Name,
                IsAuthenticated = user.Password == password,
                Role = "Organizor",
                UserId = user.Password == password ? user.OrganizerId : 0
            };
        }
    }
}
