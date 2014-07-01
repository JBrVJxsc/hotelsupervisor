namespace HotelSupervisorService.Forms.Controls.ISettingObjectEditControls
{
    partial class HotelSettingObjectEditControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HotelSettingObjectEditControl));
            this.gvMain = new HotelSupervisorService.Forms.Controls.Plus.DataGridViewPlus();
            this.chHotelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chTel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).BeginInit();
            this.SuspendLayout();
            // 
            // gvMain
            // 
            this.gvMain.AllowUserToAddRows = false;
            this.gvMain.AllowUserToDeleteRows = false;
            this.gvMain.AllowUserToResizeColumns = false;
            this.gvMain.AllowUserToResizeRows = false;
            this.gvMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gvMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gvMain.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.gvMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvMain.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chHotelName,
            this.chLocation,
            this.chTel});
            this.gvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvMain.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gvMain.Location = new System.Drawing.Point(0, 0);
            this.gvMain.MultiSelect = false;
            this.gvMain.Name = "gvMain";
            this.gvMain.NumberColumns = ((System.Collections.Generic.List<int>)(resources.GetObject("gvMain.NumberColumns")));
            this.gvMain.RowHeadersVisible = false;
            this.gvMain.RowTemplate.Height = 23;
            this.gvMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvMain.Size = new System.Drawing.Size(620, 508);
            this.gvMain.TabIndex = 0;
            this.gvMain.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvMain_CellValueChanged);
            this.gvMain.ValueChanged += new System.EventHandler(this.gvMain_ValueChanged);
            this.gvMain.AddNewRow += new System.EventHandler(this.gvMain_AddNewRow);
            // 
            // chHotelName
            // 
            this.chHotelName.HeaderText = "名称";
            this.chHotelName.Name = "chHotelName";
            this.chHotelName.ReadOnly = true;
            this.chHotelName.Width = 220;
            // 
            // chLocation
            // 
            this.chLocation.HeaderText = "地址";
            this.chLocation.Name = "chLocation";
            this.chLocation.Width = 220;
            // 
            // chTel
            // 
            this.chTel.HeaderText = "联系电话";
            this.chTel.Name = "chTel";
            this.chTel.Width = 160;
            // 
            // HotelSettingObjectEditControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gvMain);
            this.Name = "HotelSettingObjectEditControl";
            ((System.ComponentModel.ISupportInitialize)(this.gvMain)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private HotelSupervisorService.Forms.Controls.Plus.DataGridViewPlus gvMain;
        private System.Windows.Forms.DataGridViewTextBoxColumn chHotelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn chLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn chTel;

    }
}
