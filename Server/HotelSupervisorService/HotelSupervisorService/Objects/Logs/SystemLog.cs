using HotelSupervisorService.Enums;
using HotelSupervisorService.Interfaces;

namespace HotelSupervisorService.Objects.Logs
{
    public class SystemLog : BaseLog, ILog
    {
        public SystemLog()
        { 
            
        }

        public SystemLog(SystemLogType systemLogType, string content)
        {
            this.systemLogType = systemLogType;
            this.content = content;
        }

        public SystemLog(SystemLogType systemLogType, string content, object attach)
        {
            this.systemLogType = systemLogType;
            this.content = content;
            this.attach = attach;
        }

        private SystemLogType systemLogType = SystemLogType.系统;
        private string content = string.Empty;
        private object attach = null;

        public SystemLogType SystemLogType
        {
            get
            {
                return systemLogType;
            }
            set
            {
                systemLogType = value;
            }
        }

        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                content = value;
            }
        }

        public object Attach
        {
            get
            {
                return attach;
            }
            set
            {
                attach = value;
            }
        }

        #region ILog 成员

        public LogType LogType
        {
            get
            {
                return LogType.System;
            }
        }

        #endregion
    }
}
