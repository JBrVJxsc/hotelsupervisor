using System;
using System.Drawing;
using System.Windows.Forms;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Exceptions;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects.Communication;
using HotelSupervisorService.Objects.Logs;

namespace HotelSupervisorService.Objects.Command
{
    public class UpdateCommand : BaseCommand ,ICommand
    {
        private CommunicationParameter communicationParameter = new CommunicationParameter(EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.ReceiveMessageUserName), EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.ReceiveMessagePassword));
        private string targetAddress = EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.CommandUpdateTargetAddress);

        public CommandType CommandType
        {
            get
            {
                return CommandType.UPDATE;
            }
        }

        #region ICommand 成员

        public string CommandName
        {
            get
            {
                return "更新";
            }
        }

        public int CommandSortID
        {
            get
            {
                return 1;
            }
        }

        public Image CommandIcon
        {
            get
            {
                return global::HotelSupervisorService.Properties.Resources.Update;
            }
        }

        public string TargetAddress
        {
            get
            {
                return targetAddress;
            }
        }

        public CommunicationParameter CommunicationParameter
        {
            get
            {
                return communicationParameter;
            }
        }

        public void Execute()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Multiselect = true;
                openFileDialog.FileName = string.Empty;
                openFileDialog.Filter = "所有文件|*.*";
                DialogResult dr = openFileDialog.ShowDialog();
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
                dr = MessageBox.Show("确认要发送更新命令吗？","更新",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                CommandObject commandObject = new CommandObject();
                commandObject.CommandType = CommandType.UPDATE;
                communicationManager.SendCommandMessage(this, commandObject, null, openFileDialog.FileNames);
                SystemLog systemLog = new SystemLog(SystemLogType.推送更新, "发送成功。");
                LogManager.GetLogManager().AddLog(systemLog);
                DataBaseManager.GlobalDataBaseManager.InsertCommand(commandObject);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("93。", e);
            }
        }

        #endregion
    }
}
