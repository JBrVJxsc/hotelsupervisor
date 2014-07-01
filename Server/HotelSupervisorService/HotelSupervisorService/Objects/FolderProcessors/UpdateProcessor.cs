using System.Collections.Generic;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects.Command;
using HotelSupervisorService.Objects.Communication;
using HotelSupervisorService.Objects.Logs;

namespace HotelSupervisorService.Objects.FolderProcessors
{
    public class UpdateProcessor : BaseProcessor, IFolderProcessor
    {

        #region IFolderProcessor 成员

        public string OperatingFolderName
        {
            get
            {
                return "U";
            }
        }

        public string MoveTargetFolderName
        {
            get
            {
                return "UD";
            }
        }

        public void Process(List<MessageWhole> messageWholeList)
        {
            foreach (MessageWhole messageWhole in messageWholeList)
            {
                if (!messageWhole.Subject.StartsWith("&&"))
                {
                    if (DeleteMessage != null)
                    {
                        DeleteMessage(this, new MessageHandleTask(messageWhole.UID, OperatingFolderName, MoveTargetFolderName));
                    }
                    continue;
                }
                string[] subject = EncryptionManager.Decrypt(messageWhole.Subject.Replace("&&", string.Empty)).Split('&');
                if (subject[0] != "UPDATE")
                {
                    continue;
                }
                string[] body = EncryptionManager.Decrypt(messageWhole.BodyText.Split('$')[0]).Split('&');
                CommandInfoObject commandInfoObject = new CommandInfoObject();
                commandInfoObject.CommandID = subject[1];
                if (body.Length > 0)
                {
                    commandInfoObject.ResponseHotelID = body[0];
                }
                if (body.Length > 1)
                {
                    commandInfoObject.ResponseHotelName = body[1];
                }
                if (body.Length > 2)
                {
                    commandInfoObject.ResponseContent = body[2];
                }
                if (MoveMessage != null)
                {
                    MoveMessage(this, new MessageHandleTask(messageWhole.UID, OperatingFolderName, MoveTargetFolderName));
                }
                DataBaseManager.GlobalDataBaseManager.InsertCommandInfo(commandInfoObject);
                string content = "名称：" + commandInfoObject.ResponseHotelName + " 原客户端版本号：" + commandInfoObject.ResponseContent;
                SystemLog systemLog = new SystemLog(SystemLogType.客户端更新, content);
                LogManager.GetLogManager().AddLog(systemLog);
            }
        }

        public int SortID
        {
            get
            {
                return 3;
            }
        }

        public event MoveMessageHandle MoveMessage;

        public event DeleteMessageHandle DeleteMessage;

        #endregion
    }
}
