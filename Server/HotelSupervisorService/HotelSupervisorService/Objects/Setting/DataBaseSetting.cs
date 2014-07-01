using System;
using System.Drawing;
using HotelSupervisorService.Forms.Controls.ISettingObjectEditControls;
using HotelSupervisorService.Interfaces;

namespace HotelSupervisorService.Objects.Setting
{
    /// <summary>
    /// 数据库设置实体。
    /// </summary>
    public class DataBaseSetting : ISettingObject
    {
        private string compareDataBaseUrl = "DataBase\\Compare\\ztk.mdb";
        private string localDataBaseUrl = "DataBase\\Local\\localdb.mdb";
        private string compareDataBaseUserID = "Admin";
        private string localDataBaseUserID = "Admin";
        private string compareDataBasePassword = "police";
        private string localDataBasePassword = "He********";
        private DataBaseSettingObjectEditControl dataBaseSettingObjectEditControl = new DataBaseSettingObjectEditControl();

        /// <summary>
        /// 对比数据库的位置。
        /// </summary>
        public string CompareDataBaseUrl
        {
            get
            {
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase + compareDataBaseUrl;
            }
            set
            {
                compareDataBaseUrl = value;
            }
        }

        /// <summary>
        /// 本地数据库的位置。
        /// </summary>
        public string LocalDataBaseUrl
        {
            get
            {
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase + localDataBaseUrl;
            }
            set
            {
                localDataBaseUrl = value;
            }
        }

        /// <summary>
        /// 对比数据库的用户名。
        /// </summary>
        public string CompareDataBaseUserID
        {
            get
            {
                return compareDataBaseUserID;
            }
            set
            {
                compareDataBaseUserID = value;
            }
        }

        /// <summary>
        /// 本地数据库的用户名。
        /// </summary>
        public string LocalDataBaseUserID
        {
            get
            {
                return localDataBaseUserID;
            }
            set
            {
                localDataBaseUserID = value;
            }
        }

        /// <summary>
        /// 对比数据库的密码。
        /// </summary>
        public string CompareDataBasePassword
        {
            get
            {
                return compareDataBasePassword;
            }
            set
            {
                compareDataBasePassword = value;
            }
        }

        /// <summary>
        /// 本地数据库的密码。
        /// </summary>
        public string LocalDataBasePassword
        {
            get
            {
                return localDataBasePassword;
            }
            set
            {
                localDataBasePassword = value;
            }
        }

        #region ISettingObject 成员

        public string GetSettingName()
        {
            return "数据设置";
        }

        public Image GetSettingIcon()
        {
            return global::HotelSupervisorService.Properties.Resources.DataBase;
        }

        public ISettingObjectEditControl GetSettingObjectEditControl()
        {
            return dataBaseSettingObjectEditControl;
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
            return 4;
        }

        public bool Visible()
        {
            return false;
        }

        #endregion
    }
}
