using System.ServiceProcess;

namespace HotelSupervisorClient
{
    static class Program
    {
        /// <summary>
        /// 版本号。
        /// </summary>
        public static string Version = "1.10";

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
