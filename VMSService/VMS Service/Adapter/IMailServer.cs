using System;
namespace VMS_Service.Adapter
{
    public interface IMailServer
    {
        bool Send(string to, string header, string body);
    }
}