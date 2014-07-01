using System;
using System.IO;
using HotelSupervisorClient.Managers;

namespace HotelSupervisorClient.Plus
{
    internal partial class FileSystemWatcherPlus : FileSystemWatcher
    {
        public FileSystemWatcherPlus()
        {
            InitializeComponent();
        }

        public static string DataBaseFileFullName = string.Empty;

        public void Init()
        {
            string subKey = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.RegBaseLocation);
            RegistryManager localMachineManager = new RegistryManager();
            Filter = EncryptionManager.Decrypt(global::HotelSupervisorClient.Properties.Resources.DataBaseFileName);
            try
            {
                Path = localMachineManager.GetDataBaseFilePath();
            }
            catch
            {
                throw;
            }
            try
            {
                EnableRaisingEvents = true;
            }
            catch
            {
                throw new Exception("09。");
            }
            DataBaseFileFullName = Path + "\\" + Filter;
        }
    }
}
