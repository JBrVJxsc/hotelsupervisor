using System;
using System.Collections.Generic;
using System.Drawing;
using HotelSupervisorService.Forms.Controls.ISettingObjectEditControls;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Objects.Setting
{
    /// <summary>
    /// 干警设置实体。
    /// </summary>
    public class PolicemanSetting : ISettingObject
    {
        private PolicemanSettingObjectEditControl policemanSettingObjectEditControl = new PolicemanSettingObjectEditControl();
        private List<Policeman> policemanList;

        #region ISettingObject 成员

        public string GetSettingName()
        {
            return "警员设置";
        }

        public Image GetSettingIcon()
        {
            return global::HotelSupervisorService.Properties.Resources.Policeman;
        }

        public ISettingObjectEditControl GetSettingObjectEditControl()
        {
            return policemanSettingObjectEditControl;
        }

        public int InitSetting()
        {
            policemanList = DataBaseManager.GlobalDataBaseManager.GetAllPoliceman();
            return 1;
        }

        public int SaveSetting()
        {
            throw new NotImplementedException();
        }

        public int GetSortID()
        {
            return 1;
        }

        public bool Visible()
        {
            return false;
        }

        #endregion
    }
}
