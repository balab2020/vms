using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VMSConsoleStore
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new VMSConsoleStore.VMSEntities())
            {
                var firstMeeting = db.Meetings
                    .Where(m => m.MeetingId == 1)
                    .First();
                db.spCreateMeeting(1, "sangavi@gmail.com", "213456789", DateTime.Now, "from .net code");
            }
            Console.ReadLine();
        }
    }
}
