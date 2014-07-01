using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace HotelSupervisorClientUpdater.Managers
{
    internal class ServiceManager
    {
        private static string serviceName = EncryptionManager.Decrypt(global::HotelSupervisorClientUpdater.Properties.Resources.ServiceName);

        public static void StartService()
        {
            try
            {
                ServiceController serviceController = new ServiceController(serviceName);
                if ((serviceController.Status.Equals(ServiceControllerStatus.Stopped)) ||
                     (serviceController.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    serviceController.Start();
                }
            }
            catch (Exception e)
            {
                throw new Exception("24。" + e.Message);
            }
        }

        public static void StopService()
        {
            try
            {
                ServiceController serviceController = new ServiceController(serviceName);
                if ((!serviceController.Status.Equals(ServiceControllerStatus.Stopped)) &&
                                   (!serviceController.Status.Equals(ServiceControllerStatus.StopPending)))
                {
                    serviceController.Stop();
                }
            }
            catch (Exception e)
            {
                throw new Exception("25。" + e.Message);
            }
        }

        public static bool IsServiceStoped()
        {
            try
            {
                ServiceController serviceController = new ServiceController(serviceName);
                if (serviceController.Status == ServiceControllerStatus.Stopped)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception("25。" + e.Message);
            }
        }
    }
}
