namespace VMS_Service.Database
{
    public interface IVMSStore
    {
        Meeting GetMeeting(int id);

        int CreateMeeting(int visitorId, string email, string mobile, System.DateTime dateTime, string purpose);

        bool UpdateMeeting(int id, MeetingState state);
    }
}