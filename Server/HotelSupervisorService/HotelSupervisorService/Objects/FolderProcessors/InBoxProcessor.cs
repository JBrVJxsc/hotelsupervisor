using System;
using System.Collections.Generic;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects.Communication;
using HotelSupervisorService.Objects.Logs;

namespace HotelSupervisorService.Objects.FolderProcessors
{
    public class InBoxProcessor:BaseProcessor, IFolderProcessor
    {
        public static SuspectManager GlobalSuspectManager = new SuspectManager();

        #region IFolderProcessor 成员

        public string OperatingFolderName
        {
            get
            {
                return "INBOX";
            }
        }

        public string MoveTargetFolderName
        {
            get
            {
                return "SD";
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
                        DeleteMessage(this,new MessageHandleTask(messageWhole.UID,OperatingFolderName,MoveTargetFolderName));
                    }
                    continue;
                }
                string subject = EncryptionManager.Decrypt(messageWhole.Subject.Replace("&&", string.Empty));
                if (subject != "NEWGUEST")
                {
                    continue;
                }
                string[] body = EncryptionManager.Decrypt(messageWhole.BodyText.Split('$')[0]).Split('&');
                Guest guest = new Guest();
                Hotel hotel = new Hotel();
                guest.CardNumber = body[0];
                guest.Name = body[1];
                hotel.Code=body[2];
                hotel.Name = body[3];
                guest.LogRoom = body[4];
                string logTimeStr=body[5];
                string year = logTimeStr.Substring(0, 4);
                string month = logTimeStr.Substring(4, 2);
                string day = logTimeStr.Substring(6, 2);
                string hour = logTimeStr.Substring(8, 2);
                string min = logTimeStr.Substring(10, 2);
                guest.LogTime = DateTime.Parse(year + "-" + month + "-" + day + " " + hour + ":" + min + ":00").ToString();
                if (MoveMessage != null)
                {
                    MoveMessage(this, new MessageHandleTask(messageWhole.UID, OperatingFolderName, MoveTargetFolderName));
                }
                GuestLog guestLog = new GuestLog(guest, hotel);
                LogManager.GetLogManager().AddLog(guestLog);
                GlobalSuspectManager.Check(guest, hotel);
            }
        }

        public int SortID
        {
            get
            {
                return 1;
            }
        }

        public event MoveMessageHandle MoveMessage;

        public event DeleteMessageHandle DeleteMessage;

        #endregion
    }
}
