using System.Drawing;
using System.Windows.Forms;

namespace HotelSupervisorService.Forms.Controls.Plus
{
    public partial class ToolTipPlus : ToolTip
    {
        public ToolTipPlus()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            ToolTipIcon = ToolTipIcon.Info;
            ForeColor = Color.Black;
            ToolTipTitle = "提示";
            ShowAlways = true;
        }
    }
}
