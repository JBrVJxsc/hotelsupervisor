
namespace HotelSupervisorService.Objects
{
    /// <summary>
    /// 旅店。
    /// </summary>
    public class Hotel
    {
        private string code = string.Empty;
        private string name = string.Empty;
        private string location = string.Empty;
        private string mapUrl = string.Empty;
        private string hotelTel = string.Empty;
        private string hotelOwner = string.Empty;
        private string hotelOwnerTel = string.Empty;

        /// <summary>
        /// 旅店编码。
        /// </summary>
        public string Code
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }

        /// <summary>
        /// 旅店姓名。
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
        /// 旅店地址。
        /// </summary>
        public string Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        /// <summary>
        /// 地图链接。
        /// </summary>
        public string MapUrl
        {
            get
            {
                return mapUrl;
            }
            set
            {
                mapUrl = value;
            }
        }

        /// <summary>
        /// 旅店联系电话。
        /// </summary>
        public string HotelTel
        {
            get
            {
                return hotelTel;
            }
            set
            {
                hotelTel = value;
            }
        }

        /// <summary>
        /// 旅店法人。
        /// </summary>
        public string HotelOwner
        {
            get
            {
                return hotelOwner;
            }
            set
            {
                hotelOwner = value;
            }
        }

        /// <summary>
        /// 旅店法人联系方式。
        /// </summary>
        public string HotelOwnerTel
        {
            get
            {
                return hotelOwnerTel;
            }
            set
            {
                hotelOwnerTel = value;
            }
        }
    }
}
