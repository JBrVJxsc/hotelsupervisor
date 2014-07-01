using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using HotelSupervisorClientUpdater.Managers;

namespace HotelSupervisorClientUpdater
{
    internal partial class Service : ServiceBase
    {
        public Service()
        {
            InitializeComponent();
        }

        private bool processing = false;
        private System.Timers.Timer timer = new System.Timers.Timer(180000);

        protected override void OnStart(string[] args)
        {
            try
            {
                fileSystemWatcherPlus.Init();
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
                timer.Enabled = true;
                Process();
            }
            catch (Exception e)
            {
                LogManager.GetLogManager().CreateLog("错误代码：" + e.Message);
            }
            finally
            {
                processing = false;
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (processing)
            {
                return;
            }
            try
            {
                ServiceManager.StartService();
            }
            catch (Exception exception)
            {
                LogManager.GetLogManager().CreateLog("错误代码：" + exception.Message);
            }
        }

        protected override void OnStop()
        {
            while (processing)
            {
                Thread.Sleep(5000);
            }
        }

        private void Process()
        {
            processing = true;

            Thread.Sleep(3000);

            string dir = EncryptionManager.Decrypt(global::HotelSupervisorClientUpdater.Properties.Resources.UpdatePath);
            string updateFileIdentificationCode = EncryptionManager.Decrypt(global::HotelSupervisorClientUpdater.Properties.Resources.UpdateFileIdentificationCode);
            DirectoryInfo dirInfo = new DirectoryInfo(dir);
            FileInfo[] files = dirInfo.GetFiles();
            List<FileInfo> newFiles = new List<FileInfo>();
            foreach (FileInfo file in files)
            {
                if (file.Name.Contains(updateFileIdentificationCode))
                {
                    newFiles.Add(file);
                }
            }
            if (newFiles.Count == 0)
            {
                return;
            }
            ServiceManager.StopService();

            Thread.Sleep(15000);

            foreach (FileInfo file in newFiles)
            {
                string deleteFile = file.FullName.Replace(updateFileIdentificationCode, string.Empty);
                if (File.Exists(deleteFile))
                {
                    try
                    {
                        File.Delete(deleteFile);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("26。" + e.Message);
                    }
                }
                while (File.Exists(deleteFile))
                {
                    Thread.Sleep(3000);
                }
                try
                {
                    File.Move(file.FullName, file.FullName.Replace(updateFileIdentificationCode, string.Empty));
                }
                catch (Exception e)
                {
                    throw new Exception("27。" + e.Message);
                }
            }

            Thread.Sleep(3000);

            ServiceManager.StartService();
        }

        private void fileSystemWatcherPlus_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                Thread.Sleep(8000);
                Process();
            }
            catch (Exception exception)
            {
                LogManager.GetLogManager().CreateLog("错误代码：" + exception.Message);
            }
            finally
            {
                try
                {
                    ServiceManager.StartService();
                }
                catch (Exception exception)
                {
                    LogManager.GetLogManager().CreateLog("错误代码：" + exception.Message);
                }
                processing = false;
            }
        }
    }
}
