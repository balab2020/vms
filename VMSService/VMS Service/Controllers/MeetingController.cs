namespace VMS_Service.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;
    using VMS_Service.Adapter;
    using VMS_Service.Database;
    using Meeting = Models.Meeting;

    [RoutePrefix("api/meeting")]
    public class MeetingController : ApiController
    {

        private readonly IVMSStore _store;

        private readonly MailMessageBuilder _mailMessageBuilder;

        public MeetingController()
        {
            _store = new VMSStore();
            _mailMessageBuilder = new MailMessageBuilder();
        }

        [NonAction]
        private Meeting GetMeeting(int id)
        {
            var meeting = _store.GetMeeting(id);

            return new Meeting
            {
                DateTime = meeting.Date.ToString(),
                MeetingId = id,
                OrganizorId = meeting.OrganizerId,
                OrganizorName = meeting.Organizer.Name,
                Mobile = meeting.Visitor.ContactNumber,
                Purpose = meeting.Purpose,
                VisitorEmail = meeting.Visitor.EmailId,
                OTP = meeting.OTP.ToString()
            };
        }

        /// <summary>
        /// http://localhost:53781/api/meeting?id=7
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// </returns>
        [HttpGet]
        public IHttpActionResult Get(int id)
        {           
            return Ok(GetMeeting(id));
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
        public IHttpActionResult Create([FromBody]Meeting meeting)
        {
            try
            {
                var meetingid = _store.CreateMeeting(meeting.OrganizorId, meeting.VisitorEmail, meeting.Mobile, System.DateTime.Parse(meeting.DateTime), meeting.Purpose);
                var meetingCreated = GetMeeting(meetingid);
                try
                {
                    var gmail = new GmailServer();
                    gmail.Send(
                        meetingCreated.VisitorEmail,
                        _mailMessageBuilder.BuildSubject(meetingCreated.OrganizorName),
                        _mailMessageBuilder.BuildBody(meetingCreated)
                    );
                }
                catch (System.Exception)
                {
                    return Created("Meeting", meetingCreated);
                }
                return Ok(meetingCreated);
            }
            catch (System.Exception ex)
            {
            }
            return BadRequest();
        }

        /// <summary>
        /// http://localhost:53781/api/meeting/Acknowledge/1?email=bbaba@asd.com
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="email"></param>
        [HttpPut]
        [Route("Acknowledge/{meetingId}")]
        public IHttpActionResult Acknowledge(int meetingId, [FromUri] string email)
        {
            try
            {
                _store.UpdateMeeting(meetingId, MeetingState.Acknowledged, email);                
                return Ok();
            }
            catch (System.Exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotModified, new HttpError()));
            }
        }

        /// <summary>
        /// http://localhost:53781/api/meeting/Complete/1003?email=bbaba@asd.com
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="email"></param>
        [HttpPut]
        [Route("Complete/{meetingId}")]
        public IHttpActionResult Complete(int meetingId, [FromUri] string email)
        {
            try
            {
                _store.UpdateMeeting(meetingId, MeetingState.Closed, email);
                return Ok();
            }
            catch (System.Exception)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NotModified, new HttpError()));
            }
        }
    }
}
