namespace VMS_Service.Controllers
{
    using System.Web.Http;
    using VMS_Service.Database;
    using Meeting = Models.Meeting;

    [RoutePrefix("api/meeting")]
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
                DateTime = meeting.Date.ToString(),
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
            _store.CreateMeeting(meeting.OrganizorId,meeting.VisitorEmail, meeting.Mobile,System.DateTime.Parse(meeting.DateTime),meeting.Purpose);
        }

        [HttpPut]
        [Route("Acknowledge/{meetingId}")]
        public void Acknowledge(int meetingId, [FromUri] string email)
        {
            _store.UpdateMeeting(meetingId, MeetingState.Acknowledged, email);
        }

        [HttpPut]
        [Route("Complete/{meetingId}")]
        public void Complete(int meetingId, [FromUri] string email)
        {
            _store.UpdateMeeting(meetingId, MeetingState.Closed, email);
        }
    }
}
