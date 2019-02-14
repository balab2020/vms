namespace VMS_Service.Adapter
{
    using System;
    using System.Net;
    using System.Net.Mail;
   
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
                        message.Body = body;
                        message.IsBodyHtml = true;
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
    }

}