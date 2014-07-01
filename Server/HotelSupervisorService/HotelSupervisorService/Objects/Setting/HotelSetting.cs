using System.Collections.Generic;
using System.Drawing;
using HotelSupervisorService.Forms.Controls.ISettingObjectEditControls;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Objects.Setting
{
    /// <summary>
    /// 旅店信息设置。
    /// </summary>
    public class HotelSetting : ISettingObject
    {
        private HotelSettingObjectEditControl hotelSettingObjectEditControl = new HotelSettingObjectEditControl();
        private List<Hotel> hotelList;

        public List<Hotel> HotelList
        {
            get
            {
                return hotelList;
            }
            set
            {
                hotelList = value;
            }
        }

        #region ISettingObject 成员

        public string GetSettingName()
        {
            return "旅店设置";
        }

        public Image GetSettingIcon()
        {
            return global::HotelSupervisorService.Properties.Resources.Hotel;
        }

        public ISettingObjectEditControl GetSettingObjectEditControl()
        {
            return hotelSettingObjectEditControl;
        }

        public int InitSetting()
        {
            hotelList = DataBaseManager.GlobalDataBaseManager.GetAllHotelInfo();
            return 1;
        }

        public int SaveSetting()
        {
            return 1;
        }

        public int GetSortID()
        {
            return 2;
        }

        public bool Visible()
        {
            return true;
        }

        #endregion
    }
}
