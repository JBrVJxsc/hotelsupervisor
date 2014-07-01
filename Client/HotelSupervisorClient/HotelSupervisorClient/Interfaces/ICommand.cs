using System.Collections.Generic;
using System.Timers;
using HotelSupervisorClient.Objects.Communication;

namespace HotelSupervisorClient.Interfaces
{
    /// <summary>
    /// 命令均需实现此接口。
    /// </summary>
    internal interface ICommand
    {
        string UserName
        {
            get;
        }

        string Password
        {
            get;
        }

        /// <summary>
        /// 命令等级。数字越小等级越高。
        /// </summary>
        int CommandLevel
        {
            get;
        }

        /// <summary>
        /// 命令的检查频率。
        /// </summary>
        int CheckTime
        {
            get;
        }

        /// <summary>
        /// 开始检测。
        /// </summary>
        void Check(object sender, ElapsedEventArgs e);

        bool Process(List<MessageWhole> messageWholeList, bool firstReceive);

        event NeedProcessHandler NeedProcess;

        void InitCommandResponseHolder();

        void SaveCommandResponseHolder();
    }

    internal delegate void NeedProcessHandler(ICommand iCommand, List<MessageWhole> messageWholeList, bool firstReceive);
}
