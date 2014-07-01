using System.Collections.Generic;
using LumiSoft.Net.Mail;

namespace HotelSupervisorService.Objects.Communication
{
    public class MessageWhole
    {
        private Mail_Message mime;
        private long uid = -1;
        private string from = string.Empty;
        private string subject = string.Empty;
        private string bodyText = string.Empty;
        private string bodyHTMLText = string.Empty;
        private List<Attachment> attachmentList=new List<Attachment>();

        public Mail_Message Mime
        {
            get
            {
                return mime;
            }
            set
            {
                mime = value;
            }
        }

        /// <summary>
        /// UID。
        /// </summary>
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

        /// <summary>
        /// 发送者。
        /// </summary>
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

        /// <summary>
        /// 标题。
        /// </summary>
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

        /// <summary>
        /// 信息主体。
        /// </summary>
        public string BodyText
        {
            get
            {
                return bodyText;
            }
            set
            {
                bodyText = value;
            }
        }

        public string BodyHTMLText
        {
            get
            {
                return bodyHTMLText;
            }
            set
            {
                bodyHTMLText = value;
            }
        }

        public List<Attachment> AttachmentList
        {
            get
            {
                return attachmentList;
            }
            set
            {
                attachmentList = value;
            }
        }
    }
}
