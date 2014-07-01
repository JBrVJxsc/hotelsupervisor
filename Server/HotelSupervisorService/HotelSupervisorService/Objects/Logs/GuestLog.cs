using HotelSupervisorService.Enums;
using HotelSupervisorService.Interfaces;

namespace HotelSupervisorService.Objects.Logs
{
    /// <summary>
    /// 记录。
    /// </summary>
    public class GuestLog : BaseLog, ILog
    {
        public GuestLog()
        { 
            
        }

        public GuestLog(Guest guest,Hotel hotel)
        {
            this.guest = guest;
            this.hotel = hotel;
        }

        private Guest guest = new Guest();
        private Hotel hotel = new Hotel();

        public Guest Guest
        {
            get
            {
                return guest;
            }
            set
            {
                guest = value;
            }
        }

        public Hotel Hotel
        {
            get
            {
                return hotel;
            }
            set
            {
                hotel = value;
            }
        }

        #region ILog 成员

        public LogType LogType
        {
            get
            {
                return LogType.Guest;
            }
        }

        #endregion
    }
}
