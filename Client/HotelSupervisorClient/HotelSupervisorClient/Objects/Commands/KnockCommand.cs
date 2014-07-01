using System.Collections.Generic;
using HotelSupervisorClient.Interfaces;
using HotelSupervisorClient.Managers;
using HotelSupervisorClient.Objects.Communication;
using HotelSupervisorClient.Objects.Holders;

namespace HotelSupervisorClient.Objects.Commands
{
    internal class KnockCommand : BaseCommand, ICommand
    {
        private string userName = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.CommandKnockUserName);
        private string password = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.CommandKnockPassword);
        private string checkPointRegeditKeyName = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.CommandKnockCheckPointRegeditKeyName);

        public override string UserName
        {
            get
            {
                return userName;
            }
        }

        public override string Password
        {
            get
            {
                return password;
            }
        }

        protected override string CheckPointRegeditKeyName
        {
            get
            {
                return checkPointRegeditKeyName;
            }
        }

        #region ICommand 成员

        public int CommandLevel
        {
            get
            {
                return 1;
            }
        }

        public int CheckTime
        {
            get
            {
                return 45000;
            }
        }

        public bool Process(List<MessageWhole> messageWholeList, bool firstReceive)
        {
            if (firstReceive)
            {
                //2012年12月7日18:33:25修改，不加这段代码了。实际意义不是特别大，而且容易导致这家旅店彻底不再响应探测命令。
                //return true;
            }
            MessageWhole messageWhole = messageWholeList[0];
            string subject = EncryptionManager.Decrypt(messageWhole.Subject);
            string[] info = subject.Split('&');
            if (info[0] != EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.MessageCommandKnockSubject))
            {
                return true;
            }
            else
            {
                string body = Service.GlobalHotelInfo.ID + "&" + Service.GlobalHotelInfo.Name + "&" + Program.Version;
                baseResponseHolder.ResponseList.Add(new Response(messageWhole.Subject, EncryptionManager.Encrypt(body)));
                CheckHolder();
            }
            return true;
        }

        public void InitCommandResponseHolder()
        {
            baseResponseHolder = new KnockResponseHolder();
            baseResponseHolder.Init();
            CheckHolder();
        }

        #endregion
    }
}
