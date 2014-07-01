using System;
using System.Collections.Generic;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Interfaces;

namespace HotelSupervisorService.Managers
{
    /// <summary>
    /// 设置管理类。
    /// </summary>
    public class SettingManager
    {
        /// <summary>
        /// 获取当前所有存在的设置。
        /// </summary>
        /// <returns>所有存在的设置。</returns>
        public List<ISettingObject> GetSettingObjectList()
        {
            List<Type> typeList = ReflectionManager.GetTypesByInterface(typeof(ISettingObject), TypeOfType.Class);
            List<ISettingObject> iSettingObjectList = new List<ISettingObject>();
            foreach (Type type in typeList)
            {
                ISettingObject iSettingObject = ReflectionManager.CreateInstanceByType(type) as ISettingObject;
                iSettingObjectList.Add(iSettingObject);
            }
            return iSettingObjectList;
        }
    }
}
