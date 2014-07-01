using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Exceptions;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects.Logs;

namespace HotelSupervisorService.Forms.Controls.LogShowers
{
    public partial class SystemLogShower : BaseLogShower, ILogShower
    {
        public SystemLogShower()
        {
            InitializeComponent();
        }

        private ListViewItem GetListViewItem(SystemLog systemLog)
        {
            ListViewItem listViewItem = new ListViewItem(systemLog.Time.ToString());
            ListViewItem.ListViewSubItem systemLogTypeItem = new ListViewItem.ListViewSubItem(listViewItem, systemLog.SystemLogType.ToString());
            ListViewItem.ListViewSubItem logContentItem = new ListViewItem.ListViewSubItem(listViewItem, systemLog.Content);
            listViewItem.SubItems.Add(systemLogTypeItem);
            listViewItem.SubItems.Add(logContentItem);
            return listViewItem;
        }

        private void AddSystemLog(SystemLog systemLog)
        {
            ListViewItem listViewItem = GetListViewItem(systemLog);
            Items.Insert(0, listViewItem);
            if (Items.Count > MaxLogNumber)
            {
                Items.RemoveAt(Items.Count - 1);
            }
        }

        public override void AddLog(ILog iLog)
        {
            SystemLog systemLog = iLog as SystemLog;
            if (systemLog == null)
            {
                return;
            }
            try
            {
                DataBaseManager.GlobalDataBaseManager.InsertSystemLog(systemLog);
            }
            catch(Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                ListViewItem item = GetListViewItem(new SystemLog( SystemLogType.异常,"错误代码：65。"));
                Items.Insert(0, item);
                if (Items.Count > MaxLogNumber)
                {
                    Items.RemoveAt(Items.Count - 1);
                }
                LogManager.GetLogManager().CreateLog(e);
            }
            if (systemLog.SystemLogType == SystemLogType.异常 && systemLog.Attach!=null)
            {
                LogManager.GetLogManager().CreateLog(systemLog.Attach as Exception);
            }
            AddSystemLog(systemLog);
        }

        public override void DeleteLog(ILog iLog)
        {

        }

        public override void ClearLog()
        {

        }

        public override void SaveLog()
        {

        }

        public override void ShowLog(int number)
        {

        }

        #region ILogShower 成员

        public LogType LogType
        {
            get
            {
                return LogType.System;
            }
        }

        public void InitLog()
        {
            SuspendLayout();
            List<SystemLog> systemLogList = DataBaseManager.GlobalDataBaseManager.GetSystemLogByTop(MaxLogNumber);
            if (systemLogList != null)
            {
                for (int i = systemLogList.Count - 1; i >= 0; i--)
                {
                    AddSystemLog(systemLogList[i]);
                }
            }
            ResumeLayout();
        }

        public int MaxLogNumber
        {
            get
            {
                return 100;
            }
        }

        #endregion
    }
}
