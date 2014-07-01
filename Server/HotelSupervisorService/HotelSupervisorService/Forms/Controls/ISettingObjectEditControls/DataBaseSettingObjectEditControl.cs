using System;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Objects.Setting;

namespace HotelSupervisorService.Forms.Controls.ISettingObjectEditControls
{
    public partial class DataBaseSettingObjectEditControl : BaseSettingObjectEditControl, ISettingObjectEditControl
    {
        public DataBaseSettingObjectEditControl()
        {
            InitializeComponent();
        }

        private DataBaseSetting dataBaseSetting;

        protected override void InitToolStripButtons()
        {
            base.InitToolStripButtons();
        }

        #region ISettingObjectEditControl 成员

        public ISettingObject ISettingObject
        {
            get
            {
                return dataBaseSetting;
            }
            set
            {
                dataBaseSetting = value as DataBaseSetting;
            }
        }

        public void InitSettingObject(ISettingObject iSettingObject)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
