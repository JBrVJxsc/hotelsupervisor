using HotelSupervisorService.Enums;
using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Interfaces
{
    public interface ILogShower
    {
        /// <summary>
        /// 记录类型。
        /// </summary>
        LogType LogType
        {
            get;
        }

        /// <summary>
        /// 增加Log。
        /// </summary>
        /// <param name="iLog">Log实体。</param>
        void AddLog(ILog iLog);

        /// <summary>
        /// 删除Log。
        /// </summary>
        /// <param name="iLog">Log实体。</param>
        void DeleteLog(ILog iLog);

        /// <summary>
        /// 清除Log。
        /// </summary>
        void ClearLog();

        /// <summary>
        /// 保存Log。
        /// </summary>
        void SaveLog();

        /// <summary>
        /// 按照数量显示Log。
        /// </summary>
        /// <param name="number">数量。</param>
        void ShowLog(int number);

        /// <summary>
        /// 向记录管理类注册。
        /// </summary>
        /// <param name="logManager">记录管理类实体。</param>
        void RegisterLogManager(LogManager logManager);

        void InitLog();

        int MaxLogNumber
        {
            get;
        }
    }
}
