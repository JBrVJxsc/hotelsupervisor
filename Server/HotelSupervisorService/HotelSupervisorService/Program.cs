using System;
using System.Windows.Forms;
using HotelSupervisorService.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace HotelSupervisorService
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SingleInstanceManager manager = new SingleInstanceManager();
            manager.Run(new string[] {});
        }

        public class SingleInstanceManager : WindowsFormsApplicationBase
        {
            Service service;
            public SingleInstanceManager()
            {
                this.IsSingleInstance = true;
            }
            protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs e)
            {
                service = new Service();
                Application.Run(service);
                return false;
            }
            protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
            {
                base.OnStartupNextInstance(eventArgs);
                service.WindowState = FormWindowState.Maximized;
            }
        }
    }
}
