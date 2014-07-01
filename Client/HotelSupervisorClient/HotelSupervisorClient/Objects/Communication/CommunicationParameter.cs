
namespace HotelSupervisorClient.Objects.Communication
{
    /// <summary>
    /// 通信参数。
    /// </summary>
    internal class CommunicationParameter
    {
        private string userName = string.Empty;
        private string password = string.Empty;
        private bool useSSL = false;

        public CommunicationParameter(string userName,string password)
        {
            this.userName = userName;
            this.password = password;
        }

        /// <summary>
        /// 用户名。
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
            }
        }

        /// <summary>
        /// 密码。
        /// </summary>
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
            }
        }

        /// <summary>
        /// 是否使用SSL。
        /// </summary>
        public bool UseSSL
        {
            get
            {
                return useSSL;
            }
            set
            {
                useSSL = value;
            }
        }
    }
}
