using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using HotelSupervisorService.Enums;
using HotelSupervisorService.Exceptions;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;
using HotelSupervisorService.Objects.Logs;

namespace HotelSupervisorService.Forms.Controls.Plus
{
    public partial class CommandToolStripDropDownButton : ToolStripDropDownButton
    {
        public CommandToolStripDropDownButton()
        {
            InitializeComponent();
            try
            {
                Init();
            }
            catch (ExceptionPlus exception)
            {
                MessageBox.Show("错误代码：91。","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }

        private void Init()
        {
            try
            {
                List<Type> typeList = ReflectionManager.GetTypesByInterface(typeof(ICommand), TypeOfType.Class);
                ToolStripItem[] toolStripItems = new ToolStripItem[typeList.Count];
                foreach (Type type in typeList)
                {
                    ICommand iCommand = ReflectionManager.CreateInstanceByType(type) as ICommand;
                    ToolStripMenuItem toolStripMenuItem = GetToolStripMenuItem(iCommand);
                    toolStripItems[iCommand.CommandSortID] = toolStripMenuItem;
                }
                DropDownItems.AddRange(toolStripItems);
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("91。", e);
            }
        }

        private ToolStripMenuItem GetToolStripMenuItem(ICommand iCommand)
        {
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem.Size = new Size(152, 22);
            toolStripMenuItem.Image = iCommand.CommandIcon;
            toolStripMenuItem.Text = iCommand.CommandName;
            toolStripMenuItem.Tag = iCommand;
            toolStripMenuItem.Click += new EventHandler(toolStripMenuItem_Click);
            return toolStripMenuItem;
        }

        void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ((sender as ToolStripMenuItem).Tag as ICommand).Execute();
            }
            catch (ExceptionPlus exception)
            {
                LogManager.GetLogManager().AddLog(new SystemLog(SystemLogType.异常, "错误代码：" + exception.Message));
            }
        }
    }
}
