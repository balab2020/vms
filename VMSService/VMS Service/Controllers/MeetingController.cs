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

        /// <summary>
        /// http://localhost:53781/api/meeting?id=7
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// </returns>
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

        /// <summary>
        /// Url: http://localhost:53781/api/meeting/create
        /// {
        ///     "organizorId": 1,
        ///     "visitorEmail": "bala",
        ///     "mobile": "97645622",
        ///     "purpose": "From Postman",
        ///     "dateTime": "2019-04-23T18:25:43.511Z"
        ///  }
        /// </summary>
        /// <param name="meeting"></param>
        [HttpPost]
        public void Create([FromBody]Meeting meeting)
        {
            _store.CreateMeeting(meeting.OrganizorId,meeting.VisitorEmail, meeting.Mobile,System.DateTime.Parse(meeting.DateTime),meeting.Purpose);
        }

        /// <summary>
        /// http://localhost:53781/api/meeting/Acknowledge/1?email=bbaba@asd.com
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="email"></param>
        [HttpPut]
        [Route("Acknowledge/{meetingId}")]
        public void Acknowledge(int meetingId, [FromUri] string email)
        {
            _store.UpdateMeeting(meetingId, MeetingState.Acknowledged, email);
        }

        /// <summary>
        /// http://localhost:53781/api/meeting/Complete/1003?email=bbaba@asd.com
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="email"></param>
        [HttpPut]
        [Route("Complete/{meetingId}")]
        public void Complete(int meetingId, [FromUri] string email)
        {
            _store.UpdateMeeting(meetingId, MeetingState.Closed, email);
        }
    }
}
