using System;
using HotelSupervisorService.Interfaces;

namespace HotelSupervisorService.Forms.Controls.ISettingObjectEditControls
{
    public partial class CommunicationSettingObjectEditControl : BaseSettingObjectEditControl, ISettingObjectEditControl
    {
        public CommunicationSettingObjectEditControl()
        {
            InitializeComponent();
        }

        #region ISettingObjectEditControl 成员

        public ISettingObject ISettingObject
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public void InitSettingObject(ISettingObject iSettingObject)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
