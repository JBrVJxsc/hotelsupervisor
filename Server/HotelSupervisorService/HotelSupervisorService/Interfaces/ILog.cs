using System;
using HotelSupervisorService.Enums;

namespace HotelSupervisorService.Interfaces
{
    public interface ILog
    {
        /// <summary>
        /// 记录类型。
        /// </summary>
        LogType LogType
        {
            get;
        }

        /// <summary>
        /// 记录生成时间。
        /// </summary>
        DateTime Time
        {
            get;
        }
    }
}
