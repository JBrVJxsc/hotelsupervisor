using HotelSupervisorClient.Managers;

namespace HotelSupervisorClient.Objects
{
    internal class HotelInfo
    {
        public HotelInfo()
        {

        }

        private string id = string.Empty;
        private string name = string.Empty;
        private string location = string.Empty;
        private string tel = string.Empty;

        /// <summary>
        /// 旅店编号。
        /// </summary>
        public string ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }

        /// <summary>
        /// 名称。
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
        /// 地址。
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
        /// 联系电话。
        /// </summary>
        public string Tel
        {
            get
            {
                return tel;
            }
            set
            {
                tel = value;
            }
        }

        public void Init()
        {
            RegistryManager registryManager = new RegistryManager();
            registryManager.GetHotelInfo(ref id, ref name, ref location, ref tel);
        }
    }
}
