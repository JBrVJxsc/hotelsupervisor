using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using HotelSupervisorService.Exceptions;
using HotelSupervisorService.Forms.Controls.Plus;
using HotelSupervisorService.Interfaces;
using HotelSupervisorService.Managers;

namespace HotelSupervisorService.Forms.Controls
{
    public partial class SettingControl : UserControl
    {
        public SettingControl()
        {
            InitializeComponent();
        }

        private List<ButtonPlus> buttonList = new List<ButtonPlus>();
        private ButtonPlus activatedButton = null;

        public event ChangeToolStripButtonHandle ChangeToolStripButton;

        private void InitSetting()
        {
            try
            {
                SettingManager settingManager = new SettingManager();
                List<ISettingObject> iSettingObjectList = settingManager.GetSettingObjectList();
                ISettingObject[] iSettingObjects = new ISettingObject[iSettingObjectList.Count];
                foreach (ISettingObject iSettingObject in iSettingObjectList)
                {
                    iSettingObject.InitSetting();
                    iSettingObjects[iSettingObject.GetSortID()] = iSettingObject;
                }
                for (int i = 0; i < iSettingObjects.Length; i++)
                {
                    ButtonPlus button = new ButtonPlus();
                    button.Text = iSettingObjects[i].GetSettingName();
                    button.Image = iSettingObjects[i].GetSettingIcon();
                    button.Tag = iSettingObjects[i];
                    button.Click += new EventHandler(button_Click);
                    button.Visible = iSettingObjects[i].Visible();
                    buttonList.Add(button);
                    flowLayoutPanelLeft.Controls.Add(button);
                    if (i == 0)
                    {
                        button.Activated = true;
                        activatedButton = button;
                        ShowSettingControl(iSettingObjects[i]);
                    }
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("52。", e);
            }
        }

        void button_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonPlus button = sender as ButtonPlus;
                if (activatedButton == button)
                {
                    return;
                }
                ISettingObject iSettingObject = activatedButton.Tag as ISettingObject;
                if (iSettingObject.GetSettingObjectEditControl().NeedSave)
                {
                    ToolStripButton saveButton = iSettingObject.GetSettingObjectEditControl().GetSaveButton();
                    string message = iSettingObject.GetSettingName() + "的设置已被更改，请保存。";
                    WindowManager.ShowToolTip(saveButton.Owner, message, new Point(saveButton.Owner.Location.X, saveButton.Height));
                    return;
                }
                foreach (ButtonPlus buttonPlus in buttonList)
                {
                    buttonPlus.Activated = false;
                }
                button.Activated = true;
                activatedButton = button;
                ShowSettingControl(activatedButton.Tag as ISettingObject);
            }
            catch (Exception exception)
            {
                if (exception.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("54。", exception);
            }
        }

        private void ShowSettingControl(ISettingObject iSettingObject)
        {
            try
            {
                ISettingObjectEditControl iSettingObjectEditControl = iSettingObject.GetSettingObjectEditControl();
                if (iSettingObjectEditControl.ISettingObject == null)
                {
                    iSettingObjectEditControl.ISettingObject = iSettingObject;
                }
                Control control = iSettingObjectEditControl as Control;
                control.Dock = DockStyle.Fill;
                if (!pnlRight.Controls.Contains(control))
                {
                    pnlRight.Controls.Add(control);
                }
                control.BringToFront();
                if (ChangeToolStripButton != null)
                {
                    ChangeToolStripButton(iSettingObjectEditControl.GetToolStripButtons());
                }
            }
            catch (Exception e)
            {
                if (e.GetType() == typeof(ExceptionPlus))
                {
                    throw;
                }
                throw new ExceptionPlus("53。", e);
            }
        }

        private void flowLayoutPanelLeft_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Pen drawPen = new Pen(Color.Gray, 1);
            Point[] points = new Point[2];
            points[0] = new Point(flowLayoutPanelLeft.Width - 3,0);
            points[1] = new Point(flowLayoutPanelLeft.Width - 3,flowLayoutPanelLeft.Height - 1);
            e.Graphics.DrawLines(drawPen, points);
            drawPen.Dispose();
        }

        protected override void OnLoad(EventArgs e)
        {
            InitSetting();
            base.OnLoad(e);
        }
    }

    /// <summary>
    /// 需要更改工具栏按钮时执行的方法。
    /// </summary>
    /// <param name="toolStripButton">工具栏按钮。</param>
    public delegate void ChangeToolStripButtonHandle(ToolStripButton[] toolStripButtons);
}
