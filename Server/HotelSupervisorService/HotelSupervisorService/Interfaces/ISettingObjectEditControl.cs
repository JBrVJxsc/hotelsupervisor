using System.Windows.Forms;

namespace HotelSupervisorService.Interfaces
{
    /// <summary>
    /// 设置实体编辑控件接口。
    /// </summary>
    public interface ISettingObjectEditControl
    {
        /// <summary>
        /// 设置实体。
        /// </summary>
        ISettingObject ISettingObject
        {
            get;
            set;
        }

        /// <summary>
        /// 是否需要保存。
        /// </summary>
        bool NeedSave
        {
            get;
            set;
        }

        /// <summary>
        /// 获取控件所需ToolStripButton。
        /// </summary>
        /// <returns>ToolStripButton数组。</returns>
        ToolStripButton[] GetToolStripButtons();

        /// <summary>
        /// 初始化设置实体。
        /// </summary>
        /// <param name="iSettingObject">设置实体。</param>
        void InitSettingObject(ISettingObject iSettingObject);

        /// <summary>
        /// 获取控件的保存按钮。
        /// </summary>
        /// <returns>保存按钮。</returns>
        ToolStripButton GetSaveButton();
    }
}
