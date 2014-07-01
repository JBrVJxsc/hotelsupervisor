using System.Collections.Generic;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects.Communication;
using HotelSupervisorService.Objects.Logs;

namespace HotelSupervisorService.Objects.FolderProcessors
{
    public class RegisterProcessor : BaseProcessor, IFolderProcessor
    {

        #region IFolderProcessor 成员

        public string OperatingFolderName
        {
            get
            {
                return "R";
            }
        }

        public string MoveTargetFolderName
        {
            get
            {
                return "RD";
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
                string subject = EncryptionManager.Decrypt(messageWhole.Subject.Replace("&&", string.Empty));
                if (subject != "REGISTER")
                {
                    continue;
                }
                string[] body = EncryptionManager.Decrypt(messageWhole.BodyText.Split('$')[0]).Split('&');
                Hotel hotel = new Hotel();
                hotel.Code = body[0];
                hotel.Name = body[1];
                hotel.Location = body[2];
                hotel.HotelTel = body[3];
                if (MoveMessage != null)
                {
                    MoveMessage(this, new MessageHandleTask(messageWhole.UID, OperatingFolderName, MoveTargetFolderName));
                }
                DataBaseManager.GlobalDataBaseManager.InsertHotelInfo(hotel);
                string content = "名称：" + hotel.Name + " 地址：" + hotel.Location + " 联系电话：" + hotel.HotelTel;
                SystemLog systemLog = new SystemLog(SystemLogType.旅店注册, content);
                LogManager.GetLogManager().AddLog(systemLog);
            }
        }

        public int SortID
        {
            get
            {
                return 0;
            }
        }

        public event MoveMessageHandle MoveMessage;

        public event DeleteMessageHandle DeleteMessage;

        #endregion
    }
}
