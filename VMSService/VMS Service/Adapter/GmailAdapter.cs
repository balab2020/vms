namespace VMS_Service.Adapter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text.RegularExpressions;

    public class GmailServer : IMailServer
    {
        private readonly int GmailSmtpPort;

        private readonly string GmailSmtpServer;

        public GmailServer():this("smtp.gmail.com",587)
        {

        }

        public GmailServer(string smtpServer, int port)
        {
            this.GmailSmtpPort = port;
            this.GmailSmtpServer = smtpServer;
        }


        public bool Send(string to, string header, string body)
        {
            var senderEmail = "vms.psna@gmail.com";
            var receiver = new MailAddress(to, to);
            var sender = new MailAddress(senderEmail, "P.S.N.A Parent Teacher Association");
            var smtp = new SmtpClient
            {
                Host = GmailSmtpServer,
                Port = GmailSmtpPort,
                Credentials = new NetworkCredential(senderEmail, "vms@54321"),
                EnableSsl = true,
            };
            try
            {
                using (smtp)
                {
                    using (var message = new MailMessage(sender, receiver))
                    {
                        message.Subject = header;
                        message.IsBodyHtml = true;
                        message.AlternateViews.Add(ContentToAlternateView(body));
                        smtp.Send(message);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        private static AlternateView ContentToAlternateView(string content)
        {
            var imgCount = 0;
            List<LinkedResource> resourceCollection = new List<LinkedResource>();
            foreach (Match m in Regex.Matches(content, "<img(?<value>.*?)>"))
            {
                imgCount++;
                var imgContent = m.Groups["value"].Value;
                string type = Regex.Match(imgContent, ":(?<type>.*?);base64,").Groups["type"].Value;
                string base64 = Regex.Match(imgContent, "base64,(?<base64>.*?)\"").Groups["base64"].Value;
                if (String.IsNullOrEmpty(type) || String.IsNullOrEmpty(base64))
                {
                    //ignore replacement when match normal <img> tag
                    continue;
                }
                var replacement = " src=\"cid:" + imgCount + "\"";
                content = content.Replace(imgContent, replacement);
                LinkedResource tempResource = new LinkedResource(Base64ToImageStream(base64), new ContentType(type))
                {
                    ContentId = imgCount.ToString()
                };
                resourceCollection.Add(tempResource);
            }

            AlternateView alternateView = AlternateView.CreateAlternateViewFromString(content, null, MediaTypeNames.Text.Html);
            foreach (var item in resourceCollection)
            {
                alternateView.LinkedResources.Add(item);
            }

            return alternateView;
        }

        public static Stream Base64ToImageStream(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            return ms;
        }
    }

}