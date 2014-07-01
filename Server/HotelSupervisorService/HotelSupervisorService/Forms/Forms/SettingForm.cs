using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HotelSupervisorService.Exceptions;

namespace HotelSupervisorService.Forms.Forms
{
    public partial class SettingForm : Form
    {
        public SettingForm()
        {
            InitializeComponent();
        }

        private List<ToolStripButton> toolStripButtonList = new List<ToolStripButton>();

        private void settingControl_ChangeToolStripButton(ToolStripButton[] toolStripButtons)
        {
            try
            {
                foreach (ToolStripButton toolStripButton in toolStripButtonList)
                {
                    toolStripButton.Visible = false;
                }
                for (int i = 0; i < toolStripButtons.Length; i++)
                {
                    if (!msMain.Items.Contains(toolStripButtons[i]))
                    {
                        msMain.Items.Add(toolStripButtons[i]);
                        toolStripButtonList.Add(toolStripButtons[i]);
                    }
                    else
                    {
                        toolStripButtons[i].Visible = true;
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("51。", e);
            }
        }
    }
}
