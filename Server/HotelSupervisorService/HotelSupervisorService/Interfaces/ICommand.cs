using System.Drawing;
using HotelSupervisorService.Objects.Communication;

namespace HotelSupervisorService.Interfaces
{
    public interface ICommand
    {
        /// <summary>
        /// 命令名称。
        /// </summary>
        string CommandName
        {
            get;
        }

        /// <summary>
        /// 命令序号。
        /// </summary>
        int CommandSortID
        {
            get;
        }

        Image CommandIcon
        {
            get;
        }

        /// <summary>
        /// 目标地址。
        /// </summary>
        string TargetAddress
        {
            get;
        }

        /// <summary>
        /// 连接参数。
        /// </summary>
        CommunicationParameter CommunicationParameter
        {
            get;
        }

        /// <summary>
        /// 执行命令。
        /// </summary>
        void Execute();
    }
}
