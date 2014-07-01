using System.Collections.Generic;
using System.Drawing;
using HotelSupervisorService.Forms.Controls.ISettingObjectEditControls;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Objects.Setting
{
    /// <summary>
    /// 特殊监控设置。
    /// </summary>
    public class SpecialSupervisedSetting : ISettingObject
    {
        private SpecialSupervisedSettingObjectEditControl specialSupervisedSettingObjectEditControl = new SpecialSupervisedSettingObjectEditControl();
        private List<SpecialSupervised> specialSupervisedList;

        /// <summary>
        /// 特殊监控列表。
        /// </summary>
        public List<SpecialSupervised> SpecialSupervisedList
        {
            get
            {
                return specialSupervisedList;
            }
            set
            {
                specialSupervisedList = value;
            }
        }

        #region ISettingObject 成员

        public string GetSettingName()
        {
            return "特殊监控";
        }

        public Image GetSettingIcon()
        {
            return global::HotelSupervisorService.Properties.Resources.SpecialSupervised;
        }

        public ISettingObjectEditControl GetSettingObjectEditControl()
        {
            return specialSupervisedSettingObjectEditControl;
        }

        public int InitSetting()
        {
            specialSupervisedList = DataBaseManager.GlobalDataBaseManager.GetAllSpecialSupervised();
            return 1;
        }

        public int SaveSetting()
        {
            return 1;
        }

        public int GetSortID()
        {
            return 0;
        }

        public bool Visible()
        {
            return true;
        }

        #endregion
    }
}
