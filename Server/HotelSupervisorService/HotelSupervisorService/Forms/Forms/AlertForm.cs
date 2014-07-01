using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects;

namespace HotelSupervisorService.Forms.Forms
{
    public partial class AlertForm : Form
    {
        public AlertForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        private WindowManager windowManager = new WindowManager();

        public void Alert(List<Suspect> suspectList)
        {
            lvSuspect.SuspendLayout();
            foreach (Suspect suspect in suspectList)
            {
                AddListViewGroup(suspect);
            }
            lvSuspect.ResumeLayout();
        }

        public void RegisterSuspectManager(SuspectManager suspectManager)
        {
            suspectManager.Alert += Alert;
        }

        public bool Check()
        {
            List<Suspect> suspectList = DataBaseManager.GlobalDataBaseManager.GetUnHandledSuspectHistory();
            if (suspectList != null && suspectList.Count > 0)
            {
                Alert(suspectList);
                return true;
            }
            return false;
        }

        private void AddListViewGroup(Suspect suspect)
        {
            ListViewGroup listViewGroup = new ListViewGroup(suspect.Name);
            lvSuspect.Groups.Insert(0,listViewGroup);
            ListViewItem listViewItem = GetListViewItem(suspect);
            listViewItem.Group = listViewGroup;
            listViewItem.Tag = suspect;
            lvSuspect.Items.Insert(0,listViewItem);
        }

        private void CheckItems()
        {
            if (lvSuspect.Items.Count == 0)
            {
                Hide();
            }
        }

        private ListViewItem GetListViewItem(Suspect suspect)
        {
            ListViewItem listViewItem = new ListViewItem(suspect.LastAppearHotelName);
            ListViewItem.ListViewSubItem hotelRoomItem = new ListViewItem.ListViewSubItem(listViewItem, suspect.LastAppearHotelRoom);
            ListViewItem.ListViewSubItem hotelLocationItem = new ListViewItem.ListViewSubItem(listViewItem, suspect.Hotel.Location);
            ListViewItem.ListViewSubItem hotelLocationTel = new ListViewItem.ListViewSubItem(listViewItem, suspect.Hotel.HotelTel);
            ListViewItem.ListViewSubItem cardNumberItem = new ListViewItem.ListViewSubItem(listViewItem, suspect.CardNumber);
            ListViewItem.ListViewSubItem checkSourceItem = new ListViewItem.ListViewSubItem(listViewItem, suspect.CheckSource.ToString());
            listViewItem.SubItems.Add(hotelRoomItem);
            listViewItem.SubItems.Add(hotelLocationItem);
            listViewItem.SubItems.Add(hotelLocationTel);
            listViewItem.SubItems.Add(cardNumberItem);
            listViewItem.SubItems.Add(checkSourceItem);
            return listViewItem;
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
            windowManager.RemoveCloseButton(Handle.ToInt32());
            base.OnLoad(e);
        }

        private void panelWithColor_Click(object sender, EventArgs e)
        {
            Visible = false;
        }

        private void lvSuspect_ItemDoubleClick(object sender, ListViewItem listViewItem)
        {
            Suspect suspect = listViewItem.Tag as Suspect;
            if (suspect == null)
            {
                return;
            }
            DialogResult dr = MessageBox.Show("是否将“" + suspect.Name + "”置为已处理状态？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }
            if (DataBaseManager.GlobalDataBaseManager.UpdateSuspectToHandled(suspect.ID) > 0)
            {
                lvSuspect.Items.Remove(listViewItem);
            }
            CheckItems();
        }
    }
}
