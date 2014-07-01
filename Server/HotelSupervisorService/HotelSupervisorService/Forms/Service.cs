using System;
using System.Windows.Forms;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Exceptions;
using HotelSupervisorService.Forms.Forms;
using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects.Logs;

namespace HotelSupervisorService.Forms
{
    public partial class Service : Form
    {
        public Service()
        {
            InitializeComponent();
        }

        private SupervisorManager supervisorManager = new SupervisorManager();
        private WindowManager windowManager = new WindowManager();

        private void Init()
        {
            InitLogShower();
            InitSupervisorManager();
            InitWindow();
        }

        private void InitLogShower()
        {
            try
            {
                guestLogShower.RegisterLogManager(LogManager.GetLogManager());
                systemLogShower.RegisterLogManager(LogManager.GetLogManager());
                guestLogShower.InitLog();
                systemLogShower.InitLog();
            }
            catch(Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("46。", e);
            }
        }

        private void InitSupervisorManager()
        {
            try
            {
                AddOwnedForm(supervisorManager.AlertForm);
                supervisorManager.Processing += new ProcessingHandle(supervisorManager_Processing);
                supervisorManager.StartSupervise(15000);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("47。", e);
            }
        }

        void supervisorManager_Processing(object sender, bool processing)
        {
            tsbtExit.Enabled = !processing;
        }

        private void InitWindow()
        {
            try
            {
                windowManager.RemoveCloseButton(Handle.ToInt32());
                WindowState = FormWindowState.Maximized;
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("48。", e);
            }
        }

        private void CloseService()
        { 
            DataBaseManager.GlobalDataBaseManager.CloseDataBase();
            supervisorManager.StopSupervise();
        }

        private void tsbtSetting_Click(object sender, EventArgs e)
        {
            try
            {
                SettingForm settingForm = new SettingForm();
                settingForm.ShowDialog();
            }
            catch (ExceptionPlus exception)
            {
                LogManager.GetLogManager().AddLog(new SystemLog(SystemLogType.异常, "错误代码：" + exception.Message, exception.Exception));
            }
        }

        private void tsbtExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Maximized;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_NOCLOSE = 0x200;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle = cp.ClassStyle | CS_NOCLOSE;
                return cp;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Init();
            }
            catch (ExceptionPlus exception)
            {
                LogManager.GetLogManager().AddLog(new SystemLog(SystemLogType.异常, "错误代码：" + exception.Message, exception.Exception));
            }
            base.OnLoad(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                CloseService();
            }
            catch (ExceptionPlus exception)
            {
                LogManager.GetLogManager().AddLog(new SystemLog(SystemLogType.异常, "错误代码：" + exception.Message, exception.Exception));
            }
            base.OnFormClosed(e);
        }
    }
}
