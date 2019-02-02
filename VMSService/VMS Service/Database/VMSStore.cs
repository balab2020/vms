namespace VMS_Service.Database
{
    public class VMSStore : IVMSStore
    {
        public int CreateMeeting(int visitorId, string email, string mobile, System.DateTime dateTime, string purpose)
        {
            throw new System.NotImplementedException();
        }

        public Meeting GetMeeting(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateMeeting(int id, MeetingState state)
        {
            throw new System.NotImplementedException();
        }
    }
}