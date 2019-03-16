namespace VMS_Service.Database
{
    using System;
using System.Linq;
using VMSDbEntities = VMS_Service.Database.VMSEntities;

    public class VMSStore : IVMSStore
    {
        public int CreateMeeting(int organizorId, string email, string mobile, System.DateTime dateTime, string purpose)
        {
            using (var db = new VMSDbEntities())
            {
                var random = new Random();
                var otp = random.Next(100001, 999999);
                return (int)db.spCreateMeeting(organizorId, email, mobile, dateTime, purpose,otp).FirstOrDefault();
            }
        }

        public Meeting GetMeeting(int id)
        {
            using (var db = new VMSDbEntities())
            {
                var meeting = db.Meetings
                    .Where(m => m.MeetingId == id)
                    .FirstOrDefault();                
                meeting.Visitor = GetVisitor(db, meeting.VisitorId);
                meeting.Organizer = GetOrganizor(db, meeting.OrganizerId);
                return meeting;
            }
        }

        private Organizer GetOrganizor(VMSDbEntities db, int id)
        {
            return db.Organizers
                .Where(v => v.OrganizerId == id)
                .FirstOrDefault();
        }

        private Visitor GetVisitor(VMSDbEntities db, int visitorId)
        {
            return db.Visitors
                .Where(v => v.VisitorId == visitorId)
                .FirstOrDefault();
        }

        public bool UpdateMeeting(int id, MeetingState state, string email = null)
        {
            using (var db = new VMSDbEntities())
            {
                Random randomGenerator = new Random();
                var otp = state == MeetingState.Acknowledged ? randomGenerator.Next(100000,999999) : -1;
                db.spUpdateMeeting(id, (int)state, email??string.Empty, otp );   
            }
            return true;
        }

        public Organizer GetOrganizer(string email)
        {
            using (var db = new VMSDbEntities())
            {
                return db.Organizers.FirstOrDefault(o=>o.Email == email);
            }
        }
    }
}