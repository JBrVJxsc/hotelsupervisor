using System.Windows.Forms;
using HotelSupervisorService.Forms.Controls.ISettingObjectEditControls.ToolStripButtons;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Objects.Setting;

namespace HotelSupervisorService.Forms.Controls.ISettingObjectEditControls
{
    public partial class PolicemanSettingObjectEditControl : BaseSettingObjectEditControl, ISettingObjectEditControl
    {
        public PolicemanSettingObjectEditControl()
        {
            InitializeComponent();
        }

        private PolicemanSetting policemanSetting;

        protected override void InitToolStripButtons()
        {
            toolStripButtons = new ToolStripButton[2];
            toolStripButtons[0] = new ToolStripButtonNewOne();
            toolStripButtons[1] = new ToolStripButtonDeleteOne();
            base.InitToolStripButtons();
        }

        #region ISettingObjectEditControl 成员

        public ISettingObject ISettingObject
        {
            get
            {
                return policemanSetting;
            }
            set
            {
                policemanSetting = value as PolicemanSetting;
                InitSettingObject(policemanSetting);
            }
        }

        public void InitSettingObject(ISettingObject iSettingObject)
        {
            //grdPoliceman.Columns.Add(
        }

        #endregion

    }
}
