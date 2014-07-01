using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ServiceProcess;

namespace HotelSupervisorClientUpdater
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new Service() 
			};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
