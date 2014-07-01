
namespace HotelSupervisorService.Objects
{
    /// <summary>
    /// 警员。
    /// </summary>
    public class Policeman
    {
        private string name = string.Empty;
        private string mobilePhone = string.Empty;
        private bool isOnDuty = true;
        private string dutyTimeBegin = string.Empty;
        private string dutyTimeEnd = string.Empty;

        /// <summary>
        /// 姓名。
        /// </summary>
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

        /// <summary>
        /// 移动电话。
        /// </summary>
        public string MobilePhone
        {
            get
            {
                return mobilePhone;
            }
            set
            {
                mobilePhone = value;
            }
        }

        /// <summary>
        /// 是否执勤。
        /// </summary>
        public bool IsOnDuty
        {
            get
            {
                return isOnDuty;
            }
            set
            {
                isOnDuty = value;
            }
        }

        /// <summary>
        /// 执勤开始时间。
        /// </summary>
        public string DutyTimeBegin
        {
            get
            {
                return dutyTimeBegin;
            }
            set
            {
                dutyTimeBegin = value;
            }
        }

        /// <summary>
        /// 执勤结束时间。
        /// </summary>
        public string DutyTimeEnd
        {
            get
            {
                return dutyTimeEnd;
            }
            set
            {
                dutyTimeEnd = value;
            }
        }
    }
}
