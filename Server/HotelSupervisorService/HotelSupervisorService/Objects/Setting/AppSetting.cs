using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HotelSupervisorService.Interfaces;

namespace HotelSupervisorService.Objects.Setting
{
    /// <summary>
    /// 程序设置实体。
    /// </summary>
    public class AppSetting
    {
        private List<ISettingObject> iSettingObjects = new List<ISettingObject>();

        /// <summary>
        /// 设置实体列表。
        /// </summary>
        public List<ISettingObject> ISettingObjects
        {
            get
            {
                return iSettingObjects;
            }
            set
            {
                iSettingObjects = value;
            }
        }
    }
}
