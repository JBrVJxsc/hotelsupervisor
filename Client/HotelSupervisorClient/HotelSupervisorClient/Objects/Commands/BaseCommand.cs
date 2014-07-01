using System;
using System.Collections.Generic;
using System.Timers;
using HotelSupervisorClient.Interfaces;
using HotelSupervisorClient.Managers;
using HotelSupervisorClient.Objects.Communication;
using HotelSupervisorClient.Objects.Holders;

namespace HotelSupervisorClient.Objects.Commands
{
    internal class BaseCommand
    {
        private RegistryManager registryManager = new RegistryManager();
        private bool checking = false;
        protected CommunicationManager communicationManager = new CommunicationManager();
        protected BaseResponseHolder baseResponseHolder;
        protected Timer timer = new Timer(3000);
        public event NeedProcessHandler NeedProcess;

        /// <summary>
        /// 用户名。
        /// </summary>
        public virtual string UserName
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 密码。
        /// </summary>
        public virtual string Password
        {
            get
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 检查点的键值名称。
        /// </summary>
        protected virtual string CheckPointRegeditKeyName
        {
            get
            {
                return string.Empty;
            }
        }

        protected void CheckHolder()
        {
            if (baseResponseHolder.ResponseList.Count > 0)
            {
                timer.Enabled = true;
                timer.Elapsed -= timer_Elapsed;
                timer.Elapsed += timer_Elapsed;
            }
            else
            {
                timer.Enabled = false;
            }
        }

        protected void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool sendSuccess = true;
            try
            {
                timer.Enabled = false;
                Response response = baseResponseHolder.ResponseList[0];
                communicationManager.SendCommandMessage(this as ICommand, response.Subject, response.Body);
                baseResponseHolder.ResponseList.RemoveAt(0);
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("DT:SPM"))
                {
                    sendSuccess = false;
                    LogManager.GlobalLogManager.CreateLog("处理信息出错。错误代码：45。");
                }
                else
                {
                    LogManager.GlobalLogManager.CreateLog("处理信息出错。错误代码：" + exception.Message);
                }
            }
            finally
            {
                if (!sendSuccess)
                {
                    timer.Interval = 125000;
                }
                else
                {
                    timer.Interval = 3000;
                }
                CheckHolder();
            }
        }

        public void Check(object sender, ElapsedEventArgs e)
        {
            if (checking)
            {
                return;
            }
            try
            {
                checking = true;
                CommunicationParameter parameter = new CommunicationParameter(UserName, Password);
                if (!communicationManager.Connect(parameter))
                {
                    return;
                }
                bool firstReceive = false;
                MessageHeader messageHeader = communicationManager.GetLastMessageHeader();
                if (messageHeader == null)
                {
                    return;
                }
                if (!messageHeader.Subject.StartsWith("&&"))
                {
                    return;
                }
                string lastCheckPoint = registryManager.GetLastCommandCheckPoint(CheckPointRegeditKeyName);
                if (lastCheckPoint == null)
                {
                    registryManager.SetLastCommandCheckPoint(CheckPointRegeditKeyName, messageHeader.UID.ToString());
                    firstReceive = true;
                }
                else if (messageHeader.UID.ToString() == lastCheckPoint)
                {
                    return;
                }
                registryManager.SetLastCommandCheckPoint(CheckPointRegeditKeyName, messageHeader.UID.ToString());
                MessageWhole messageWhole = communicationManager.GetLastMessageWhole();
                List<MessageWhole> messageWholeList = new List<MessageWhole>();
                messageWhole.Subject = messageWhole.Subject.Replace("&&", string.Empty);
                messageWholeList.Add(messageWhole);
                if (NeedProcess != null)
                {
                    NeedProcess(this as ICommand, messageWholeList, firstReceive);
                }
            }
            catch
            {
                LogManager.GlobalLogManager.CreateLog("命令执行出错。错误代码：108。");
            }
            finally
            {
                checking = false;
            }
        }

        public void SaveCommandResponseHolder()
        {
            baseResponseHolder.SaveToFile();
        }
    }
}
