using System;
using System.Linq;

namespace VMS_Service.Database
{
    public class VMSStore : IVMSStore
    {
        public void CreateMeeting(int organizorId, string email, string mobile, System.DateTime dateTime, string purpose)
        {
            using (var db = new VMSDbEntities())
            {
                db.spCreateMeeting(organizorId, email, mobile, dateTime, purpose);
            }
        }

        public Meeting GetMeeting(int id)
        {
            using (var db = new VMSDbEntities())
            {
                var meeting =  db.Meetings
                    .Where(m => m.MeetingId == id)
                    .FirstOrDefault();
                meeting.Visitor = db.Visitors
                    .Where(v => v.VisitorId == meeting.VisitorId)
                    .FirstOrDefault();
                return meeting;
            }
        }

        public bool UpdateMeeting(int id, MeetingState state, string email = null)
        {
            using (var db = new VMSDbEntities())
            {
                Random randomGenerator = new Random();
                var otp = state == MeetingState.Acknowledged ? randomGenerator.Next(100000,999999) : -1;
                db.spUpdateMeeting(id, state.ToString(), email??string.Empty, otp );   
            }
            return true;
        }
    }
}