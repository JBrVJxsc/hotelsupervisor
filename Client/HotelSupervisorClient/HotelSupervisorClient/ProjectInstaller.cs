using System.ComponentModel;
using System.Configuration.Install;
using System.Management;


namespace HotelSupervisorClient
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void SetServiceDesktopInsteract(string serviceName)
        {
            ManagementObject wmiService = new ManagementObject(string.Format("Win32_Service.Name='{0}'", serviceName));
            ManagementBaseObject changeMethod = wmiService.GetMethodParameters("Change");
            changeMethod["DesktopInteract"] = true;
            ManagementBaseObject outParam = wmiService.InvokeMethod("Change", changeMethod, null);
        }

        private void serviceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            SetServiceDesktopInsteract(serviceInstaller.ServiceName);
        }
    }
}
