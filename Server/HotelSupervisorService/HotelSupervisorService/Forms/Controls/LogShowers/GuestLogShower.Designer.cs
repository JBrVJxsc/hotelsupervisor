namespace HotelSupervisorService.Forms.Controls.LogShowers
{
    partial class GuestLogShower
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
            this.chGuestType = new System.Windows.Forms.ColumnHeader();
            this.chGuestName = new System.Windows.Forms.ColumnHeader();
            this.chGuestCardNumber = new System.Windows.Forms.ColumnHeader();
            this.chHotelName = new System.Windows.Forms.ColumnHeader();
            this.chHotelRoom = new System.Windows.Forms.ColumnHeader();
            this.chLogTime = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // chGuestType
            // 
            this.chGuestType.Text = "登记类型";
            this.chGuestType.Width = 80;
            // 
            // chGuestName
            // 
            this.chGuestName.Text = "姓名";
            this.chGuestName.Width = 120;
            // 
            // chGuestCardNumber
            // 
            this.chGuestCardNumber.Text = "身份证号";
            this.chGuestCardNumber.Width = 180;
            // 
            // chHotelName
            // 
            this.chHotelName.Text = "旅店名称";
            this.chHotelName.Width = 180;
            // 
            // chHotelRoom
            // 
            this.chHotelRoom.Text = "登记房间";
            this.chHotelRoom.Width = 120;
            // 
            // chLogTime
            // 
            this.chLogTime.Text = "登记时间";
            this.chLogTime.Width = 140;
            // 
            // GuestLogShower
            // 
            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chGuestType,
            this.chGuestCardNumber,
            this.chGuestName,
            this.chHotelName,
            this.chHotelRoom,
            this.chLogTime});
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader chGuestType;
        private System.Windows.Forms.ColumnHeader chGuestName;
        private System.Windows.Forms.ColumnHeader chGuestCardNumber;
        private System.Windows.Forms.ColumnHeader chHotelName;
        private System.Windows.Forms.ColumnHeader chHotelRoom;
        private System.Windows.Forms.ColumnHeader chLogTime;

    }
}
