
namespace HotelSupervisorService.Objects
{
    /// <summary>
    /// 特殊监控。
    /// </summary>
    public class SpecialSupervised
    {
        private string cardNumber = string.Empty;
        private string name = string.Empty;
        private bool fuzzyCompare = false;
        private bool needSMSAlert = true;

        /// <summary>
        /// 身份证号。
        /// </summary>
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
        /// 模糊对比。
        /// </summary>
        public bool FuzzyCompare
        {
            get
            {
                return fuzzyCompare;
            }
            set
            {
                fuzzyCompare = value;
            }
        }

        /// <summary>
        /// 是否信息提醒。
        /// </summary>
        public bool NeedSMSAlert
        {
            get
            {
                return needSMSAlert;
            }
            set
            {
                needSMSAlert = value;
            }
        }
    }
}
