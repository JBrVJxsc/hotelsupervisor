using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HotelSupervisorService.Forms.Controls.Plus
{
    public partial class ButtonPlus : Button
    {
        public ButtonPlus()
        {
            InitializeComponent();
        }

        private bool activated = false;
        private Font unActivatedFont = new Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        private Font activatedFont = new Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

        /// <summary>
        /// 激活状态。
        /// </summary>
        public bool Activated
        {
            get
            {
                return activated;
            }
            set
            {
                activated = value;
                if (activated)
                {
                    Font = activatedFont;
                }
                else
                {
                    Font = unActivatedFont;
                }
                Invalidate();
            }
        }

        protected override bool ShowFocusCues
        {
            get
            {
                return false;
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Pen clearPen = new Pen(BackColor, 3);
            Pen drawPen = new Pen(Color.Gray, 1);
            pevent.Graphics.DrawRectangle(clearPen, 0, 0, Width - 1, Height - 1);
            if (activated)
            {
                Point[] arrowPoints = new Point[5];
                arrowPoints[0] = new Point(Width-0, 0);
                arrowPoints[1] = new Point(Width-0, Height / 2 - 8);
                arrowPoints[2] = new Point(Width-0 - 10, Height / 2);
                arrowPoints[3] = new Point(Width-0, Height / 2 + 8);
                arrowPoints[4] = new Point(Width-0, Height-2);
                //pevent.Graphics.DrawLines(drawPen, arrowPoints);
                using (Brush brush = new SolidBrush(Color.Gray))
                {
                    pevent.Graphics.FillPolygon(brush, arrowPoints);
                }
            }
            else
            {
                Point[] arrowPoints = new Point[2];
                arrowPoints[0] = new Point(Width - 0, 0);
                arrowPoints[1] = new Point(Width - 0, Height-2);
                pevent.Graphics.DrawLines(drawPen, arrowPoints);
            }
            clearPen.Dispose();
            drawPen.Dispose();
        }
    }
}
