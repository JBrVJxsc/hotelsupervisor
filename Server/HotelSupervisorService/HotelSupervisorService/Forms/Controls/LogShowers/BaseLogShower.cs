using HotelSupervisorService.Forms.Controls.Plus;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Forms.Controls.LogShowers
{
    public partial class BaseLogShower : LogListView
    {
        public BaseLogShower()
        {
            InitializeComponent();
        }

        public virtual void AddLog(ILog iLog)
        {

        }

        public virtual void DeleteLog(ILog iLog)
        {
            
        }

        public virtual void ClearLog()
        {
           
        }

        public virtual void SaveLog()
        {
           
        }

        public virtual void ShowLog(int number)
        {
            
        }

        public void RegisterLogManager(LogManager logManager)
        {
            logManager.OnAddLog += AddLog;
            logManager.OnDeleteLog += DeleteLog;
            logManager.OnClearLog += ClearLog;
            logManager.OnSaveLog += SaveLog;
            logManager.OnShowLog += ShowLog;
        }
    }
}
