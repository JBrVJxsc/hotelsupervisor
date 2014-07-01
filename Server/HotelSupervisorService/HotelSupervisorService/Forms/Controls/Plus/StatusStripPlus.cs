using System;
using System.Windows.Forms;

namespace HotelSupervisorService.Forms.Controls.Plus
{
    public partial class StatusStripPlus : StatusStrip
    {
        public StatusStripPlus()
        {
            InitializeComponent();
            lbSystemTime.Text = "系统时间：" + DateTime.Now.ToString();
            timer.Start();
        }

        private DateTime runTime = DateTime.Now;

        private void timer_Tick(object sender, EventArgs e)
        {
            lbSystemTime.Text = "系统时间：" + DateTime.Now.ToString();
        }
    }
}
