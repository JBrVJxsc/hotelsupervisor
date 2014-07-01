
namespace HotelSupervisorClient.Objects.Commands
{
    public class Response
    {
        public Response()
        { 
            
        }

        public Response(string subject, string body)
        {
            this.subject = subject;
            this.body = body;
        }

        private string subject = string.Empty;
        private string body = string.Empty;

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

        public string Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }
        }
    }
}
