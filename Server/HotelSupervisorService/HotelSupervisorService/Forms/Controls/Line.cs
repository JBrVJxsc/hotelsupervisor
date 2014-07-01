using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace HotelSupervisorService.Forms.Controls
{
    public partial class Line : UserControl
    {
        public Line()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.FixedHeight | ControlStyles.FixedWidth | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
        }

        private DashStyle mDashStyle = DashStyle.Solid;
        private Color m_FirstColor = Color.FromKnownColor(KnownColor.ControlDark);
        private Color m_SecondColor = Color.FromKnownColor(KnownColor.ControlLightLight);
        private LineStyle m_LineStyle = LineStyle.Horizontal;


        public DashStyle DashStyle
        {
            get 
            { 
                return mDashStyle; 
            }
            set
            { 
                mDashStyle = value; 
                Invalidate();
            }
        }

        public Color FirstColor
        {
            get
            {
                return m_FirstColor; 
            }
            set
            { 
                m_FirstColor = value; 
                Invalidate();
            }
        }

        public Color SecondColor
        {
            get 
            { 
                return m_SecondColor;
            }
            set
            {
                m_SecondColor = value; 
                Invalidate();
            }
        }

        public LineStyle LineStyle
        {
            get 
            { 
                return m_LineStyle;
            }
            set
            {
                m_LineStyle = value;
                Size = new Size(Height, Width);
                Invalidate();
            }
        }

        private void ChangeControlSize()
        {
            if (LineStyle == LineStyle.Horizontal)
            {
                Height = 2;
            }
            else if (LineStyle == LineStyle.Vertical)
            {
                Width = 2;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int x1, x2, x3, x4, y1, y2, y3, y4;
            if (LineStyle == LineStyle.Horizontal)
            {
                x1 = 0;
                y1 = 0;
                x2 = ClientRectangle.Width;
                y2 = 0;

                x3 = 0;
                y3 = 1;
                x4 = ClientRectangle.Width;
                y4 = 1;
            }
            else
            {
                x1 = 0;
                y1 = 0;
                x2 = 0;
                y2 = ClientRectangle.Height;

                x3 = 1;
                y3 = 0;
                x4 = 1;
                y4 = ClientRectangle.Height;
            }

            using (Pen p = new Pen(m_FirstColor, 1))
            {
                p.DashStyle = mDashStyle;
                e.Graphics.DrawLine(p, x1, y1, x2, y2);
            }
            using (Pen p = new Pen(m_SecondColor, 1))
            {
                p.DashStyle = mDashStyle;
                e.Graphics.DrawLine(p, x3, y3, x4, y4);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ChangeControlSize();
        }
    }

    public enum LineStyle
    {
        Horizontal = 1,
        Vertical = 2
    }
}
