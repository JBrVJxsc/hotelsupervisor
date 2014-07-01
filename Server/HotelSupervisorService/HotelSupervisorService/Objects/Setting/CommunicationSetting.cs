using System;
using System.Drawing;
using HotelSupervisorService.Interfaces;

namespace HotelSupervisorService.Objects.Setting
{
    /// <summary>
    /// 通信设置实体。
    /// </summary>
    public class CommunicationSetting : ISettingObject
    {
        private string receiveServerUrl = "";
        private string sendServerUrl = "";
        private int refreshCounter = 10000;

        /// <summary>
        /// 接收服务器地址。
        /// </summary>
        public string ReceiveServerUrl
        {
            get
            {
                return receiveServerUrl;
            }
            set
            {
                receiveServerUrl = value;
            }
        }

        /// <summary>
        /// 发送服务器地址。
        /// </summary>
        public string SendServerUrl
        {
            get
            {
                return sendServerUrl;
            }
            set
            {
                sendServerUrl = value;
            }
        }

        /// <summary>
        /// 刷新时间。
        /// </summary>
        public int RefreshCounter
        {
            get
            {
                return refreshCounter;
            }
            set
            {
                refreshCounter = value;
            }
        }

        #region ISettingObject 成员

        public string GetSettingName()
        {
            return "通信设置";
        }

        public Image GetSettingIcon()
        {
            return global::HotelSupervisorService.Properties.Resources.Communication;
        }

        public ISettingObjectEditControl GetSettingObjectEditControl()
        {
            throw new NotImplementedException();
        }

        public int InitSetting()
        {
            return 1;
        }

        public int SaveSetting()
        {
            throw new NotImplementedException();
        }

        public int GetSortID()
        {
            return 3;
        }

        public bool Visible()
        {
            return false;
        }

        #endregion
    }
}
