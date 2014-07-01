namespace HotelSupervisorClient
{
    partial class Service
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
            this.fileSystemWatcherPlus = new HotelSupervisorClient.Plus.FileSystemWatcherPlus();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcherPlus)).BeginInit();
            // 
            // fileSystemWatcherPlus
            // 
            this.fileSystemWatcherPlus.EnableRaisingEvents = true;
            this.fileSystemWatcherPlus.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            this.fileSystemWatcherPlus.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcherPlus_Changed);
            // 
            // Service
            // 
            this.ServiceName = "HotelSupervisor Client";
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcherPlus)).EndInit();

        }

        #endregion

        private HotelSupervisorClient.Plus.FileSystemWatcherPlus fileSystemWatcherPlus;


    }
}
