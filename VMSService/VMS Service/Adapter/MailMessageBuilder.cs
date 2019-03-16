namespace VMS_Service.Adapter
{
    using System.Text;
    using VMS_Service.Controllers;
    using VMS_Service.Models;

    public class MailMessageBuilder
    {
        public string BuildBody(Meeting meeting)
        {
            var ipServer = System.Configuration.ConfigurationManager.AppSettings["ServerIp"];
            var url = $"http://{ipServer}/api/meeting/Acknowledge/${meeting.MeetingId}?email={meeting.VisitorEmail}";
            var message = $"<p> Your are being invited to meet <strong>{meeting.OrganizorName}</strong> for discussing about {meeting.Purpose}  which scheduled at { meeting.DateTime.ToString()}.</ p >";
            var barcode = BarcodeController.GenerateQrcodeBase64(meeting.OTP);
            var mailBody = new StringBuilder();
            mailBody.Append("<html><body>");
            mailBody.Append($"<p>Dear {meeting.VisitorEmail},</p>");
            mailBody.Append($"<p>You are being invited to meet Mr.{meeting.OrganizorName.ToUpper()} for {meeting.Purpose}.</p>");
            mailBody.Append($"<p>Meeting (Id:<strong>{meeting.MeetingId}</strong>) is scheduled at <strong>{meeting.DateTime.ToString()}</strong></p>");
            mailBody.Append($"<p>Please show this mail at security gate, click <a href=\"{url}\">here</a> to confirm your visit.</p>");
            mailBody.Append($"<br/><img src=\"data:image/bitmap;base64,{barcode}\"/>");
            mailBody.Append("<br/><p>Thanks & Regards,</p>");
            mailBody.Append("<a href='http://www.psnacet.edu.in/'>");
            mailBody.Append("<span>PSNA CET Team</span>");
            mailBody.Append("</a>");
            mailBody.Append("</body></html>");
            var body = mailBody.ToString();
            return body;
        }

        public string BuildSubject(string organizerName)
        {
            return @"P.S.N.A: You are being invited to meet " + organizerName;
        }
    }
}