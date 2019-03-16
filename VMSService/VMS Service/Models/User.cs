using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMS_Service.Models
{
    public class User
    {
        public bool IsAuthenticated { get; set; }

        public string UserName { get; set; }

        public string  Role { get; set; }

        public int UserId { get; set; }
    }
}