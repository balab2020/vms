using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VMS_Service.Models
{
    public class Meeting
    {
        public int MeetingId { get; set; }

        public int OrganizorId { get; set; }

        public string VisitorEmail { get; set; }

        public string Mobile { get; set; }

        public string Purpose { get; set; }

        public DateTime DateTime { get; set; }
    }
}