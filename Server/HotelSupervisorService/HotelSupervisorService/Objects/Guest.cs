using HotelSupervisorService.Enums;

namespace HotelSupervisorService.Objects
{
    public class Guest
    {
        private GuestType guestType = GuestType.入住;
        private string cardNumber = string.Empty;
        private string name = string.Empty;
        private string logTime = string.Empty;
        private string logRoom = string.Empty;

        public GuestType GuestType
        {
            get
            {
                return guestType;
            }
            set
            {
                guestType = value;
            }
        }

        public string CardNumber
        {
            get
            {
                return cardNumber;
            }
            set
            {
                cardNumber = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public string LogTime
        {
            get
            {
                return logTime;
            }
            set
            {
                logTime = value;
            }
        }

        public string LogRoom
        {
            get
            {
                return logRoom;
            }
            set
            {
                logRoom = value;
            }
        }
    }
}
