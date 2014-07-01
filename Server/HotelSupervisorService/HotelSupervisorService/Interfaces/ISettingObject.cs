using System.Drawing;

namespace HotelSupervisorService.Interfaces
{
    /// <summary>
    /// 设置类接口。
    /// </summary>
    public interface ISettingObject
    {
        /// <summary>
        /// 获取设置名称。
        /// </summary>
        /// <returns>设置名称。</returns>
        string GetSettingName();

        /// <summary>
        /// 获取设置图标。
        /// </summary>
        /// <returns>设置图标。</returns>
        Image GetSettingIcon();

        /// <summary>
        /// 获取设置实体的编辑控件。
        /// </summary>
        /// <returns>设置实体的编辑控件。</returns>
        ISettingObjectEditControl GetSettingObjectEditControl();

        /// <summary>
        /// 初始化设置实体。
        /// </summary>
        /// <returns>1为初始化成功；0为初始化失败。</returns>
        int InitSetting();

        /// <summary>
        /// 保存设置实体。
        /// </summary>
        /// <returns>1为保存成功；0为保存失败。</returns>
        int SaveSetting();

        /// <summary>
        /// 获得序号。
        /// </summary>
        /// <returns>序号。</returns>
        int GetSortID();

        /// <summary>
        /// 是否可见。
        /// </summary>
        /// <returns>true为可见；false为不可见。</returns>
        bool Visible();
    }
}
