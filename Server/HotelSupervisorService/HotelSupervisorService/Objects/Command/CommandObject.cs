using System;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Objects.Command
{
    public class CommandObject
    {
        public CommandObject()
        {
            commandID = GuidManager.GetNewGuid(); 
        }

        private string commandID = string.Empty;
        private CommandType commandType = CommandType.KNOCK;
        private DateTime sendTime = DateTime.Now;

        /// <summary>
        /// 命令编号。
        /// </summary>
        public string CommandID
        {
            get
            {
                return commandID;
            }
        }

        /// <summary>
        /// 命令类型。
        /// </summary>
        public CommandType CommandType
        {
            get
            {
                return commandType;
            }
            set
            {
                commandType = value;
            }
        }

        public DateTime SendTime
        {
            get
            {
                return sendTime;
            }
            set
            {
                sendTime = value;
            }
        }
    }
}
