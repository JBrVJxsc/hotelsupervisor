using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HotelSupervisorService.Forms.Controls.Plus
{
    public partial class PanelWithColor : Panel
    {
        public PanelWithColor()
        {
            InitializeComponent();
        }

        private Color startColor;
        private Color endColor;
        private LinearGradientMode linearGradientMode = LinearGradientMode.Vertical;

        /// <summary>
        /// 起始颜色。
        /// </summary>
        public Color StartColor
        {
            get
            {
                return startColor;
            }
            set
            {
                startColor = value;
            }
        }

        /// <summary>
        /// 结束颜色。
        /// </summary>
        public Color EndColor
        {
            get
            {
                return endColor;
            }
            set
            {
                endColor = value;
            }
        }

        /// <summary>
        /// 渐变色的延伸方式。
        /// </summary>
        public LinearGradientMode LinearGradientMode
        {
            get
            {
                return linearGradientMode;
            }
            set
            {
                linearGradientMode = value;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (base.ClientRectangle.IsEmpty)
            {
                return;
            }

            using (Brush brush = new LinearGradientBrush(base.ClientRectangle, startColor, endColor, linearGradientMode))
            {
                e.Graphics.FillRectangle(brush, base.ClientRectangle);
            }

            base.OnPaint(e);
        }
    }
}
