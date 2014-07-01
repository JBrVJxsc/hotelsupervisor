
namespace HotelSupervisorService.Objects.Communication
{
    public class MessageHandleTask
    {
        public MessageHandleTask()
        { 
            
        }

        public MessageHandleTask(long uid,string from,string to)
        {
            this.uid = uid;
            this.from = from;
            this.to = to;
        }

        private long uid = 0;
        private string from = string.Empty;
        private string to = string.Empty;

        public long UID
        {
            get
            {
                return uid;
            }
            set
            {
                uid = value;
            }
        }

        public string From
        {
            get
            {
                return from;
            }
            set
            {
                from = value;
            }
        }

        public string To
        {
            get
            {
                return to;
            }
            set
            {
                to = value;
            }
        }
    }
}
