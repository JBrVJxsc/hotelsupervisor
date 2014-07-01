using System;

namespace HotelSupervisorService.Objects.Logs
{
    public class BaseLog
    {
        private DateTime time = DateTime.Now;

        public DateTime Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
            }
        }
    }
}
