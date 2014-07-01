namespace HotelSupervisorService.Forms
{
    partial class Service
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Service));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbtSetting = new System.Windows.Forms.ToolStripButton();
            this.tsbtQuery = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtExit = new System.Windows.Forms.ToolStripButton();
            this.tsddbtCommand = new HotelSupervisorService.Forms.Controls.Plus.CommandToolStripDropDownButton();
            this.imageListForTabControl = new System.Windows.Forms.ImageList(this.components);
            this.tbLog = new System.Windows.Forms.TabControl();
            this.tabPageGuestLog = new System.Windows.Forms.TabPage();
            this.guestLogShower = new HotelSupervisorService.Forms.Controls.LogShowers.GuestLogShower();
            this.tabPageSystemLog = new System.Windows.Forms.TabPage();
            this.systemLogShower = new HotelSupervisorService.Forms.Controls.LogShowers.SystemLogShower();
            this.statusStrip = new HotelSupervisorService.Forms.Controls.Plus.StatusStripPlus();
            this.tsMain.SuspendLayout();
            this.tbLog.SuspendLayout();
            this.tabPageGuestLog.SuspendLayout();
            this.tabPageSystemLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "监控运行中";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // tsMain
            // 
            this.tsMain.AllowMerge = false;
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtSetting,
            this.tsbtQuery,
            this.toolStripSeparator1,
            this.tsbtExit,
            this.tsddbtCommand});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(883, 39);
            this.tsMain.TabIndex = 2;
            // 
            // tsbtSetting
            // 
            this.tsbtSetting.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtSetting.Image = global::HotelSupervisorService.Properties.Resources.Setting;
            this.tsbtSetting.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtSetting.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtSetting.Name = "tsbtSetting";
            this.tsbtSetting.Size = new System.Drawing.Size(36, 36);
            this.tsbtSetting.Text = "设置";
            this.tsbtSetting.Click += new System.EventHandler(this.tsbtSetting_Click);
            // 
            // tsbtQuery
            // 
            this.tsbtQuery.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtQuery.Image = global::HotelSupervisorService.Properties.Resources.Query;
            this.tsbtQuery.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtQuery.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtQuery.Name = "tsbtQuery";
            this.tsbtQuery.Size = new System.Drawing.Size(36, 36);
            this.tsbtQuery.Text = "查询";
            this.tsbtQuery.Visible = false;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 39);
            // 
            // tsbtExit
            // 
            this.tsbtExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtExit.Image = global::HotelSupervisorService.Properties.Resources.Exit;
            this.tsbtExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbtExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtExit.Name = "tsbtExit";
            this.tsbtExit.Size = new System.Drawing.Size(36, 36);
            this.tsbtExit.Text = "退出";
            this.tsbtExit.Click += new System.EventHandler(this.tsbtExit_Click);
            // 
            // tsddbtCommand
            // 
            this.tsddbtCommand.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsddbtCommand.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsddbtCommand.Image = global::HotelSupervisorService.Properties.Resources.Broadcast;
            this.tsddbtCommand.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsddbtCommand.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtCommand.Name = "tsddbtCommand";
            this.tsddbtCommand.Size = new System.Drawing.Size(45, 36);
            this.tsddbtCommand.Text = "命令";
            // 
            // imageListForTabControl
            // 
            this.imageListForTabControl.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListForTabControl.ImageStream")));
            this.imageListForTabControl.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListForTabControl.Images.SetKeyName(0, "SystemLog.png");
            this.imageListForTabControl.Images.SetKeyName(1, "GuestLog.png");
            // 
            // tbLog
            // 
            this.tbLog.Controls.Add(this.tabPageGuestLog);
            this.tbLog.Controls.Add(this.tabPageSystemLog);
            this.tbLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLog.ImageList = this.imageListForTabControl;
            this.tbLog.Location = new System.Drawing.Point(0, 39);
            this.tbLog.Name = "tbLog";
            this.tbLog.SelectedIndex = 0;
            this.tbLog.Size = new System.Drawing.Size(883, 463);
            this.tbLog.TabIndex = 5;
            // 
            // tabPageGuestLog
            // 
            this.tabPageGuestLog.Controls.Add(this.guestLogShower);
            this.tabPageGuestLog.ImageIndex = 1;
            this.tabPageGuestLog.Location = new System.Drawing.Point(4, 25);
            this.tabPageGuestLog.Name = "tabPageGuestLog";
            this.tabPageGuestLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGuestLog.Size = new System.Drawing.Size(875, 434);
            this.tabPageGuestLog.TabIndex = 0;
            this.tabPageGuestLog.Text = "旅客日志";
            this.tabPageGuestLog.UseVisualStyleBackColor = true;
            // 
            // guestLogShower
            // 
            this.guestLogShower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.guestLogShower.FullRowSelect = true;
            this.guestLogShower.HideSelection = false;
            this.guestLogShower.Location = new System.Drawing.Point(3, 3);
            this.guestLogShower.Name = "guestLogShower";
            this.guestLogShower.Size = new System.Drawing.Size(869, 428);
            this.guestLogShower.TabIndex = 1;
            this.guestLogShower.UseCompatibleStateImageBehavior = false;
            this.guestLogShower.View = System.Windows.Forms.View.Details;
            // 
            // tabPageSystemLog
            // 
            this.tabPageSystemLog.Controls.Add(this.systemLogShower);
            this.tabPageSystemLog.ImageIndex = 0;
            this.tabPageSystemLog.Location = new System.Drawing.Point(4, 25);
            this.tabPageSystemLog.Name = "tabPageSystemLog";
            this.tabPageSystemLog.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSystemLog.Size = new System.Drawing.Size(875, 434);
            this.tabPageSystemLog.TabIndex = 1;
            this.tabPageSystemLog.Text = "系统日志";
            this.tabPageSystemLog.UseVisualStyleBackColor = true;
            // 
            // systemLogShower
            // 
            this.systemLogShower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.systemLogShower.FullRowSelect = true;
            this.systemLogShower.HideSelection = false;
            this.systemLogShower.Location = new System.Drawing.Point(3, 3);
            this.systemLogShower.Name = "systemLogShower";
            this.systemLogShower.Size = new System.Drawing.Size(869, 428);
            this.systemLogShower.TabIndex = 0;
            this.systemLogShower.UseCompatibleStateImageBehavior = false;
            this.systemLogShower.View = System.Windows.Forms.View.Details;
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 502);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(883, 22);
            this.statusStrip.TabIndex = 4;
            this.statusStrip.Text = "statusStrip1";
            // 
            // Service
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 524);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.tsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Service";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "科区公安局-独立情报系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.tbLog.ResumeLayout(false);
            this.tabPageGuestLog.ResumeLayout(false);
            this.tabPageSystemLog.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbtSetting;
        private System.Windows.Forms.ToolStripButton tsbtQuery;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbtExit;
        private HotelSupervisorService.Forms.Controls.Plus.CommandToolStripDropDownButton tsddbtCommand;
        private System.Windows.Forms.ImageList imageListForTabControl;
        private HotelSupervisorService.Forms.Controls.Plus.StatusStripPlus statusStrip;
        private System.Windows.Forms.TabControl tbLog;
        private System.Windows.Forms.TabPage tabPageGuestLog;
        private HotelSupervisorService.Forms.Controls.LogShowers.GuestLogShower guestLogShower;
        private System.Windows.Forms.TabPage tabPageSystemLog;
        private HotelSupervisorService.Forms.Controls.LogShowers.SystemLogShower systemLogShower;
    }
}