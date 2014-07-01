using System;
using System.Collections.Generic;
using System.Threading;
using System.Timers;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Exceptions;
using HotelSupervisorService.Forms.Forms;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Objects;
using HotelSupervisorService.Objects.Communication;
using HotelSupervisorService.Objects.FolderProcessors;
using HotelSupervisorService.Objects.Logs;

namespace HotelSupervisorService.Managers
{
    public class SupervisorManager
    {
        private System.Timers.Timer timer = null;
        private IFolderProcessor[] iFolderProcessors=null;
        private CommunicationManager communicationManager = new CommunicationManager();
        private SoundManager soundManager = new SoundManager();
        private CommunicationParameter communicationParameter= new CommunicationParameter(EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.ReceiveMessageUserName),EncryptionManager.Decrypt(global::HotelSupervisorService.Properties.Resources.ReceiveMessagePassword));
        private List<MessageWhole> messageWholeList;
        private AlertForm alertForm = new AlertForm();
        private int refreshTime = 15000;
        public event ProcessingHandle Processing;

        public AlertForm AlertForm
        {
            get
            {
                return alertForm;
            }
        }

        private void Check()
        {
            if (Processing != null)
            {
                Processing(this, true);
            }
            try
            {
                if (!communicationManager.Connect(communicationParameter))
                {
                    return;
                }
                for (int i = 0; i < iFolderProcessors.Length; i++)
                {
                    messageWholeList = communicationManager.GetAllMessage(iFolderProcessors[i]);
                    if (messageWholeList != null)
                    {
                        iFolderProcessors[i].Process(messageWholeList);
                    }
                }
            }
            catch (Exception exception)
            {
                if (exception.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("82。", exception);
            }
            finally
            {
                if (Processing != null)
                {
                    Processing(this, false);
                }
            }
        }

        private void InitTimer()
        {
            timer = new System.Timers.Timer();
            timer.Interval = refreshTime;
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
        }

        private void InitAlertForm()
        {
            if (alertForm.Check())
            {
                Thread thread = new Thread(ActivateAlertForm);
                thread.Start();
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timer.Enabled = false;
                Check();
            }
            catch (Exception exception)
            {
                if (exception.GetType() == typeof(ExceptionPlus))
                {
                    LogManager.GetLogManager().AddLog(new SystemLog(SystemLogType.异常, "错误代码：" + (exception as ExceptionPlus).Message, (exception as ExceptionPlus).Exception));
                }
            }
            finally
            {
                timer.Enabled = true;
            }
        }

        private void GetIFolderProcessorList()
        {
            try
            {
                List<Type> typeList = ReflectionManager.GetTypesByInterface(typeof(IFolderProcessor), TypeOfType.Class);
                iFolderProcessors = new IFolderProcessor[typeList.Count];
                foreach (Type type in typeList)
                {
                    IFolderProcessor iFolderProcessor = ReflectionManager.CreateInstanceByType(type) as IFolderProcessor;
                    iFolderProcessor.MoveMessage += new MoveMessageHandle(iFolderProcessor_MoveMessage);
                    iFolderProcessor.DeleteMessage += new DeleteMessageHandle(iFolderProcessor_DeleteMessage);
                    iFolderProcessors[iFolderProcessor.SortID] = iFolderProcessor;
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("81。", e);
            }
        }

        private void ActivateAlertForm()
        {
            soundManager.PlayAlert();
            if (!alertForm.Visible)
            {
                alertForm.Owner.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                alertForm.ShowDialog();
            }
        }

        void iFolderProcessor_DeleteMessage(IFolderProcessor iFolderProcessor, MessageHandleTask messageHandleTask)
        {
            try
            {
                communicationManager.DeleteMessage(messageHandleTask.UID, messageHandleTask.From);
            }
            catch(Exception e)
            {
                LogManager.GetLogManager().AddLog(new SystemLog(SystemLogType.异常, "错误代码：88。", e));
            }
        }

        void iFolderProcessor_MoveMessage(IFolderProcessor iFolderProcessor, MessageHandleTask messageHandleTask)
        {
            try
            {
                communicationManager.MoveMessage(messageHandleTask.UID, messageHandleTask.From, messageHandleTask.To);
            }
            catch (Exception e)
            {
                LogManager.GetLogManager().AddLog(new SystemLog(SystemLogType.异常, "错误代码：89。", e));
            }
        }

        public void StartSupervise(int refreshTime)
        {
            GetIFolderProcessorList();
            InBoxProcessor.GlobalSuspectManager.Alert += new AlertHandle(GlobalSuspectManager_Alert);
            this.refreshTime = refreshTime;
            InitAlertForm();
            InitTimer();
        }

        void GlobalSuspectManager_Alert(List<Suspect> suspectList)
        {
            alertForm.Alert(suspectList);
            Thread thread = new Thread(ActivateAlertForm);
            thread.Start();
        }

        public void StopSupervise()
        {
            timer.Enabled = false;
        }
    }

    public delegate void ProcessingHandle(object sender,bool processing);
}
