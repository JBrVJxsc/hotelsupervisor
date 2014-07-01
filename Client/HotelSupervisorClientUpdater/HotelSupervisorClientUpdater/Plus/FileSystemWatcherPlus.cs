using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using HotelSupervisorClientUpdater.Managers;

namespace HotelSupervisorClientUpdater.Plus
{
    internal partial class FileSystemWatcherPlus : FileSystemWatcher
    {
        public FileSystemWatcherPlus()
        {
            InitializeComponent();
        }

        public void Init()
        {
            Path = EncryptionManager.Decrypt(global::HotelSupervisorClientUpdater.Properties.Resources.UpdatePath);
            try
            {
                EnableRaisingEvents = true;
            }
            catch
            {
                throw new Exception("33。");
            }
        }
    }
}
