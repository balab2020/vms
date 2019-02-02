namespace VMS_Service.Controllers
{
    using System.Web.Http;
    using VMS_Service.Database;
    using Meeting = VMS_Service.Models.Meeting;

    public class MeetingController : ApiController
    {

        private readonly IVMSStore _store;

        public MeetingController()
        {
            _store = new VMSStore();
        }

        [HttpGet]
        public Meeting GetMeeting(int id)
        {
            var meeting = _store.GetMeeting(id);

            return new Meeting
            {
                DateTime = meeting.Date,
                MeetingId = id,
                OrganizorId = meeting.OrganizerId,
                Mobile = meeting.Visitor.ContactNumber,
                Purpose = meeting.Purpose,
                VisitorEmail = meeting.Visitor.EmailId
            };
        }

        [HttpPost]
        public void Create([FromBody]Meeting meeting)
        {
            _store.CreateMeeting(meeting.OrganizorId,meeting.VisitorEmail, meeting.Mobile,meeting.DateTime,meeting.Purpose);
        }

        [HttpPut]
        public void Acknowledge([FromUri]int meetingId)
        {
            _store.UpdateMeeting(meetingId, MeetingState.Acknowledged);
        }

        [HttpPut]
        public void Cancel([FromUri]int meetingId, [FromUri]string userEmail)
        {
            _store.UpdateMeeting(meetingId, MeetingState.Cancelled);
        }

        [HttpPut]
        public void Reject([FromUri]int meetingId, [FromUri]string userEmail)
        {
            _store.UpdateMeeting(meetingId, MeetingState.Rejected);
        }

        [HttpPut]
        public void Complete([FromUri]int meetingId, [FromUri]string userEmail)
        {
            _store.UpdateMeeting(meetingId, MeetingState.Closed);
        }
    }
}
