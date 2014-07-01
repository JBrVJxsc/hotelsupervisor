namespace HotelSupervisorService.Forms.Controls.LogShowers
{
    partial class BaseLogShower
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
            this.chTime = new System.Windows.Forms.ColumnHeader();
            this.SuspendLayout();
            // 
            // chTime
            // 
            this.chTime.Text = "日志时间";
            this.chTime.Width = 140;
            // 
            // BaseLogShower
            // 
            this.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTime});
            this.HideSelection = false;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader chTime;
    }
}
