using System;

namespace HotelSupervisorService.Objects.Command
{
    public class CommandInfoObject
    {
        private string commandID = string.Empty;
        private string responseHotelID = string.Empty;
        private string responseHotelName = string.Empty;
        private string responseContent = string.Empty;
        private DateTime responseTime = DateTime.Now;

        /// <summary>
        /// 命令编号。
        /// </summary>
        public string CommandID
        {
            get
            {
                return commandID;
            }
            set
            {
                commandID = value;
            }
        }

        public string ResponseHotelID
        {
            get
            {
                return responseHotelID;
            }
            set
            {
                responseHotelID = value;
            }
        }

        public string ResponseHotelName
        {
            get
            {
                return responseHotelName;
            }
            set
            {
                responseHotelName = value;
            }
        }

        public string ResponseContent
        {
            get
            {
                return responseContent;
            }
            set
            {
                responseContent = value;
            }
        }

        public DateTime ResponseTime
        {
            get
            {
                return responseTime;
            }
            set
            {
                responseTime = value;
            }
        }
    }
}
