using System.Collections.Generic;
using HotelSupervisorService.Objects.Communication;

namespace HotelSupervisorService.Interfaces
{
    public interface IFolderProcessor
    {
        /// <summary>
        /// 所要操作的文件夹名称。
        /// </summary>
        string OperatingFolderName
        {
            get;
        }

        string MoveTargetFolderName
        {
            get;
        }

        void Process(List<MessageWhole> messageWholeList);

        int SortID
        {
            get;
        }

        event MoveMessageHandle MoveMessage;
        event DeleteMessageHandle DeleteMessage;
    }

    public delegate void MoveMessageHandle(IFolderProcessor iFolderProcessor, MessageHandleTask messageHandleTask);
    public delegate void DeleteMessageHandle(IFolderProcessor iFolderProcessor, MessageHandleTask messageHandleTask);
}
