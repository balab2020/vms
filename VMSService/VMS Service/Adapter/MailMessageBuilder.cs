namespace VMS_Service.Adapter
{
    using System.Text;
    using VMS_Service.Models;

    public class MailMessageBuilder
    {
        public string BuildBody(Meeting meeting)
        {
            var ipServer = System.Configuration.ConfigurationManager.AppSettings["ServerIp"];
            var url = $"http://${ipServer}/api/meeting/Acknowledge/${meeting.MeetingId}?email={meeting.VisitorEmail}";
            var message = $"<p> Your are being invited to meet < strong >{meeting.OrganizorName}</ strong >for discussing about {meeting.Purpose}            which scheduled at { meeting.DateTime.ToString()}.</ p >";
            var mailBody = new StringBuilder();
            mailBody.Append("<html>");
            mailBody.Append("<body>");
            mailBody.Append($"<p>Hi {meeting.VisitorEmail},</p>");
            mailBody.Append(message);
            mailBody.Append("<p>&nbsp;</p>");
            mailBody.Append($"<p>Please acknowledge by clicking <a href=\"{url}\" target=\"_blank\" rel=\"noopener\">here.</a></p>");
            mailBody.Append("<p>&nbsp;</p");
            mailBody.Append("<p>Thanks,</p>");
            mailBody.Append("<p>P.S.N.A Teacher Association Team.</p>");
            mailBody.Append("</body>");
            mailBody.Append("</html>");
            return mailBody.ToString();
        }

        public string BuildSubject(string organizerName)
        {
            return @"P.S.N.A: You are being invited to meet " + organizerName;
        }
    }
}