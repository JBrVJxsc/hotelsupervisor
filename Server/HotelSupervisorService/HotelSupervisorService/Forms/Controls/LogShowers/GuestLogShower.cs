using System.Collections.Generic;
using System.Windows.Forms;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects.Logs;

namespace HotelSupervisorService.Forms.Controls.LogShowers
{
    public partial class GuestLogShower : BaseLogShower , ILogShower
    {
        public GuestLogShower()
        {
            InitializeComponent();
        }

        private ListViewItem GetListViewItem(GuestLog guestLog)
        {
            ListViewItem listViewItem = new ListViewItem(guestLog.Time.ToString());
            ListViewItem.ListViewSubItem guestTypeItem = new ListViewItem.ListViewSubItem(listViewItem, guestLog.Guest.GuestType.ToString());
            ListViewItem.ListViewSubItem guestCardIDItem = new ListViewItem.ListViewSubItem(listViewItem, guestLog.Guest.CardNumber);
            ListViewItem.ListViewSubItem guestNameItem = new ListViewItem.ListViewSubItem(listViewItem, guestLog.Guest.Name);
            ListViewItem.ListViewSubItem hotelNameItem = new ListViewItem.ListViewSubItem(listViewItem, guestLog.Hotel.Name);
            ListViewItem.ListViewSubItem hotelRoomItem = new ListViewItem.ListViewSubItem(listViewItem, guestLog.Guest.LogRoom);
            ListViewItem.ListViewSubItem hotelLogTimeItem = new ListViewItem.ListViewSubItem(listViewItem, guestLog.Guest.LogTime.ToString());
            listViewItem.SubItems.Add(guestTypeItem);
            listViewItem.SubItems.Add(guestCardIDItem);
            listViewItem.SubItems.Add(guestNameItem);
            listViewItem.SubItems.Add(hotelNameItem);
            listViewItem.SubItems.Add(hotelRoomItem);
            listViewItem.SubItems.Add(hotelLogTimeItem);
            listViewItem.Tag = guestLog;
            return listViewItem;
        }

        private void AddGuestLog(GuestLog guestLog)
        {
            ListViewItem listViewItem = GetListViewItem(guestLog);
            Items.Insert(0, listViewItem);
            if (Items.Count > MaxLogNumber)
            {
                Items.RemoveAt(Items.Count - 1);
            }
        }

        public override void AddLog(ILog iLog)
        {
            GuestLog guestLog = iLog as GuestLog;
            if (guestLog == null)
            {
                return;
            }
            DataBaseManager.GlobalDataBaseManager.InsertGuestLog(guestLog);
            AddGuestLog(guestLog);
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
                return LogType.Guest;
            }
        }

        public void InitLog()
        {
            SuspendLayout();
            List<GuestLog> guestLogList = DataBaseManager.GlobalDataBaseManager.GetGuestLogByTop(MaxLogNumber);
            if (guestLogList != null)
            {
                for (int i = guestLogList.Count - 1; i >= 0; i--)
                {
                    AddGuestLog(guestLogList[i]);
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
