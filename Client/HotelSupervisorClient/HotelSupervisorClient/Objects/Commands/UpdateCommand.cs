using System;
using System.Collections.Generic;
using System.IO;
using HotelSupervisorClient.Interfaces;
using HotelSupervisorClient.Managers;
using HotelSupervisorClient.Objects.Communication;
using HotelSupervisorClient.Objects.Holders;

namespace HotelSupervisorClient.Objects.Commands
{
    internal class UpdateCommand : BaseCommand, ICommand
    {
        private string userName = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.CommandUpdateUserName);
        private string password = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.CommandUpdatePassword);
        private string checkPointRegeditKeyName = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.CommandUpdateCheckPointRegeditKeyName);

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
                return 0;
            }
        }

        public int CheckTime
        {
            get
            {
                return 65000;
            }
        }

        public bool Process(List<MessageWhole> messageWholeList, bool firstReceive)
        {
            MessageWhole messageWhole = messageWholeList[0];
            string subject = EncryptionManager.Decrypt(messageWhole.Subject);
            string[] info = subject.Split('&');
            if (info[0] != EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.MessageCommandUpdateSubject))
            {
                return true;
            }
            else
            {
                string body = Service.GlobalHotelInfo.ID + "&" + Service.GlobalHotelInfo.Name + "&" + Program.Version;
                baseResponseHolder.ResponseList.Add(new Response(messageWhole.Subject, EncryptionManager.Encrypt(body)));
                CheckHolder();
            }

            string updaterTempName = string.Empty;
            try
            {
                string updateFilePath = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.UpdateFilePath);
                string updateFileIdentificationCode = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.UpdateFileIdentificationCode);
                foreach (Attachment attachment in messageWhole.AttachmentList)
                {
                    attachment.SaveToFile(updateFilePath + attachment.Text + updateFileIdentificationCode);
                    File.SetAttributes(updateFilePath + attachment.Text + updateFileIdentificationCode, FileAttributes.Hidden);
                }
            }
            catch (Exception e)
            {
                throw new Exception("32。" + e.Message);
            }
            return true;
        }

        public void InitCommandResponseHolder()
        {
            baseResponseHolder = new UpdateResponseHolder();
            baseResponseHolder.Init();
            CheckHolder();
        }

        #endregion
    }
}
