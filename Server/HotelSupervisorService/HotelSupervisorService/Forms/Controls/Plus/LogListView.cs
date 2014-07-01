using System.Windows.Forms;

namespace HotelSupervisorService.Forms.Controls.Plus
{
    public partial class LogListView : ListView
    {
        public event ItemDoubleClickHandle ItemDoubleClick;

        public LogListView()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.EnableNotifyMessage, true);
        }

        protected override void OnNotifyMessage(Message m)
        {
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            ListViewHitTestInfo listViewHitTestInfo = HitTest(e.X, e.Y);
            if (listViewHitTestInfo.Item != null && ItemDoubleClick!=null)
            {
                ItemDoubleClick(this, listViewHitTestInfo.Item);
            } 
            base.OnMouseDoubleClick(e);
        }
    }

    public delegate void ItemDoubleClickHandle(object sender,ListViewItem listViewItem);
}
