using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Timers;
using HotelSupervisorClient.Managers;
using HotelSupervisorClient.Objects;
using HotelSupervisorClient.Objects.Holders;

namespace HotelSupervisorClient
{
    internal partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        public static HotelInfo GlobalHotelInfo = new HotelInfo();
        private CommunicationManager communicationManager = new CommunicationManager();
        private CommandManager commandManager = new CommandManager();
        private Timer timer = new Timer(3000);
        private LocalGuestHolder localGuestHolder = new LocalGuestHolder();

        protected override void OnStart(string[] args)
        {
            try
            {
                GlobalHotelInfo.Init();
                fileSystemWatcherPlus.Init();
                commandManager.Init();
                localGuestHolder.Init();
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                CheckHolder();
            }
            catch (Exception e)
            {
                LogManager.GlobalLogManager.CreateLog("监控启动出错。错误代码：" + e.Message);
                return;
            }
            LogManager.GlobalLogManager.CreateLog("监控启动。版本："+ Program.Version);
        }

        protected override void OnStop()
        {
            try
            {
                commandManager.StopCommand();
                timer.Enabled = false;
                localGuestHolder.SaveToFile();
            }
            catch (Exception e)
            {
                LogManager.GlobalLogManager.CreateLog("监控停止出错。错误代码：" + e.Message);
                return;
            }
            LogManager.GlobalLogManager.CreateLog("监控停止。");
        }

        private void CheckHolder()
        {
            if (localGuestHolder.LocalGuestList.Count > 0)
            {
                timer.Enabled = true;
            }
            else
            {
                timer.Enabled = false;
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool sendSuccess = true;
            try
            {
                timer.Enabled = false;
                communicationManager.SendNewGuestMessage(localGuestHolder.LocalGuestList[0], GlobalHotelInfo);
                localGuestHolder.LocalGuestList.RemoveAt(0);
            }
            catch (Exception exception)
            {
                if (exception.Message.Contains("DT:SPM"))
                {
                    sendSuccess = false;
                    LogManager.GlobalLogManager.CreateLog("处理信息出错。错误代码：44。");
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

        void fileSystemWatcherPlus_Changed(object sender, FileSystemEventArgs e)
        {
            try
            {
                DataBaseManager.GlobalDataBaseManager.OpenDataBase();
                List<LocalGuest> newGuestList = DataBaseManager.GlobalDataBaseManager.GetLastLocalGuest();
                if (newGuestList != null)
                {
                    foreach (LocalGuest localGuest in newGuestList)
                    {
                        localGuestHolder.LocalGuestList.Add(localGuest);
                    }
                }
                DataBaseManager.GlobalDataBaseManager.CloseDataBase();
            }
            catch (Exception exception)
            {
                LogManager.GlobalLogManager.CreateLog("处理信息出错。错误代码：" + exception.Message);
            }
            finally
            {
                CheckHolder();
            }
        }
    }
}
