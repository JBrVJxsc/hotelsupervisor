namespace HotelSupervisorService.Forms.Forms
{
    partial class AlertForm
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
            this.lvSuspect = new HotelSupervisorService.Forms.Controls.Plus.LogListView();
            this.chHotelName = new System.Windows.Forms.ColumnHeader();
            this.chHotelRoom = new System.Windows.Forms.ColumnHeader();
            this.chHotelLocation = new System.Windows.Forms.ColumnHeader();
            this.chHotelTel = new System.Windows.Forms.ColumnHeader();
            this.chCardNumber = new System.Windows.Forms.ColumnHeader();
            this.chCheckSource = new System.Windows.Forms.ColumnHeader();
            this.panelWithColor = new HotelSupervisorService.Forms.Controls.Plus.PanelWithColor();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelWithColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lvSuspect
            // 
            this.lvSuspect.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chHotelName,
            this.chHotelRoom,
            this.chHotelLocation,
            this.chHotelTel,
            this.chCardNumber,
            this.chCheckSource});
            this.lvSuspect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSuspect.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lvSuspect.FullRowSelect = true;
            this.lvSuspect.Location = new System.Drawing.Point(0, 81);
            this.lvSuspect.Name = "lvSuspect";
            this.lvSuspect.Size = new System.Drawing.Size(794, 461);
            this.lvSuspect.TabIndex = 1;
            this.lvSuspect.UseCompatibleStateImageBehavior = false;
            this.lvSuspect.View = System.Windows.Forms.View.Details;
            this.lvSuspect.ItemDoubleClick += new HotelSupervisorService.Forms.Controls.Plus.ItemDoubleClickHandle(this.lvSuspect_ItemDoubleClick);
            // 
            // chHotelName
            // 
            this.chHotelName.Text = "旅店名称";
            this.chHotelName.Width = 160;
            // 
            // chHotelRoom
            // 
            this.chHotelRoom.Text = "房间";
            this.chHotelRoom.Width = 50;
            // 
            // chHotelLocation
            // 
            this.chHotelLocation.Text = "旅店地址";
            this.chHotelLocation.Width = 250;
            // 
            // chHotelTel
            // 
            this.chHotelTel.Text = "旅店电话";
            this.chHotelTel.Width = 90;
            // 
            // chCardNumber
            // 
            this.chCardNumber.Text = "身份证号";
            this.chCardNumber.Width = 125;
            // 
            // chCheckSource
            // 
            this.chCheckSource.Text = "辨别来源";
            this.chCheckSource.Width = 80;
            // 
            // panelWithColor
            // 
            this.panelWithColor.Controls.Add(this.pictureBox1);
            this.panelWithColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelWithColor.EndColor = System.Drawing.Color.Red;
            this.panelWithColor.LinearGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.panelWithColor.Location = new System.Drawing.Point(0, 0);
            this.panelWithColor.Name = "panelWithColor";
            this.panelWithColor.Size = new System.Drawing.Size(794, 81);
            this.panelWithColor.StartColor = System.Drawing.Color.Empty;
            this.panelWithColor.TabIndex = 0;
            this.panelWithColor.Click += new System.EventHandler(this.panelWithColor_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::HotelSupervisorService.Properties.Resources.Alert;
            this.pictureBox1.Location = new System.Drawing.Point(11, 8);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // AlertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 542);
            this.Controls.Add(this.lvSuspect);
            this.Controls.Add(this.panelWithColor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AlertForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alert!";
            this.panelWithColor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private HotelSupervisorService.Forms.Controls.Plus.PanelWithColor panelWithColor;
        private System.Windows.Forms.PictureBox pictureBox1;
        private HotelSupervisorService.Forms.Controls.Plus.LogListView lvSuspect;
        private System.Windows.Forms.ColumnHeader chHotelName;
        private System.Windows.Forms.ColumnHeader chHotelRoom;
        private System.Windows.Forms.ColumnHeader chHotelLocation;
        private System.Windows.Forms.ColumnHeader chHotelTel;
        private System.Windows.Forms.ColumnHeader chCardNumber;
        private System.Windows.Forms.ColumnHeader chCheckSource;
    }
}