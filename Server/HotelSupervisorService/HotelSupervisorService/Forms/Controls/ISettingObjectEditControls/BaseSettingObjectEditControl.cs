using System;
using System.Drawing;
using System.Windows.Forms;
using HotelSupervisorService.Forms.Controls.ISettingObjectEditControls.ToolStripButtons;
using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Forms.Controls.ISettingObjectEditControls
{
    public partial class BaseSettingObjectEditControl : UserControl
    {
        public BaseSettingObjectEditControl()
        {
            InitializeComponent();
            InitToolStripButtons();
        }

        protected ToolStripButton[] toolStripButtons=new ToolStripButton[0];
        private bool controlLoaded = false;
        private ToolStripButtonSave toolStripButtonSave;
        private bool needSave = false;

        public bool NeedSave
        {
            get
            {
                return needSave;
            }
            set
            {
                needSave = value;
                toolStripButtonSave.Enabled = needSave;
            }
        }

        /// <summary>
        /// 控件是否加载完毕。
        /// </summary>
        public bool ControlLoaded
        {
            get
            {
                return controlLoaded;
            }
        }

        protected virtual int OnSave(ref string message)
        {
            return 1;
        }

        protected virtual void InitToolStripButtons()
        {
            toolStripButtonSave = new ToolStripButtonSave();
            toolStripButtonSave.Click += new EventHandler(toolStripButtonSave_Click);
            NeedSave = false;
        }

        public ToolStripButton GetSaveButton()
        {
            return toolStripButtonSave;
        }

        void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            string message=string.Empty;
            int i = OnSave(ref message);
            if (i < 0)
            {
                return;
            }
            WindowManager.ShowToolTip(toolStripButtonSave.Owner, message, new Point(toolStripButtonSave.Owner.Location.X, toolStripButtonSave.Height));
        }

        public ToolStripButton[] GetToolStripButtons()
        {
            ToolStripButton[] tempToolStripButtons=new ToolStripButton[toolStripButtons.Length+1];
            tempToolStripButtons[0] = toolStripButtonSave;
            for (int i = 0; i < toolStripButtons.Length; i++)
            {
                tempToolStripButtons[i + 1] = toolStripButtons[i];
            }
            return tempToolStripButtons;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            controlLoaded = true;
        }
    }
}
