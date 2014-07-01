
namespace HotelSupervisorService.Objects.Communication
{
    /// <summary>
    /// 信息头。
    /// </summary>
    public class MessageHeader
    {
        private string from = string.Empty;
        private string subject = string.Empty;
        private string received = string.Empty;
        private string size = string.Empty;
        private long uid = 0;

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

        public string Subject
        {
            get
            {
                return subject;
            }
            set
            {
                subject = value;
            }
        }

        public string Received
        {
            get
            {
                return received;
            }
            set
            {
                received = value;
            }
        }

        public string Size
        {
            get
            {
                return size;
            }
            set
            {
                size = value;
            }
        }

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
    }
}
