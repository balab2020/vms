using System;
using System.Collections.Generic;
namespace VMS_Service.Adapter
{
    public interface IMailMessageBuilder
    {
        string BuildBody(int meetingId, string organizerName);
        string BuildSubject(string organizerName);
    }
}