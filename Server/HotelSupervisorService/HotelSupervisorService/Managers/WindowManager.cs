using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using HotelSupervisorService.Forms.Controls.Plus;

namespace HotelSupervisorService.Managers
{
    /// <summary>
    /// 窗体操作类。
    /// </summary>
    public class WindowManager
    {
        [DllImport("USER32.DLL")]
        public static extern int GetSystemMenu(int hwnd, int bRevert);
        [DllImport("USER32.DLL")]
        public static extern int RemoveMenu(int hMenu, int nPosition, int wFlags);

        /// <summary>
        /// 去除窗口的关闭按钮。
        /// </summary>
        /// <param name="iHWND">窗口句柄。</param>
        /// <returns>是否成功。</returns>
        public int RemoveCloseButton(int iHWND)
        {
            int iSysMenu;
            const int MF_BYCOMMAND = 0x400; 
            iSysMenu = GetSystemMenu(iHWND, 0);
            return RemoveMenu(iSysMenu, 6, MF_BYCOMMAND);
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(MouseEventFlag flags, int dx, int dy, uint data, UIntPtr extraInfo);
        [Flags]
        enum MouseEventFlag : uint
        {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }

        /// <summary>
        /// 让鼠标执行一次单击操作。
        /// </summary>
        public static void MouseClick()
        {
            mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, UIntPtr.Zero);
            mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, UIntPtr.Zero);
        }

        private static ToolTipPlus toolTip = new ToolTipPlus();

        /// <summary>
        /// 清空上一个显示的ToolTipPlus。
        /// </summary>
        private static void ClearToolTip()
        {
            toolTip.Dispose();
            toolTip = new ToolTipPlus();
        }

        /// <summary>
        /// 显示一个气泡提示。
        /// </summary>
        /// <param name="c">显示所位于的控件。</param>
        /// <param name="message">信息。</param>
        public static void ShowToolTip(Control c, string message)
        {
            ClearToolTip();
            toolTip.Show(message, c, 5000);
        }

        /// <summary>
        /// 显示一个气泡提示。
        /// </summary>
        /// <param name="c">显示所位于的控件</param>
        /// <param name="message">信息。</param>
        /// <param name="focus">显示后，是否将焦点置于控件上。</param>
        public static void ShowToolTip(Control c, string message, bool focus)
        {
            ClearToolTip();
            toolTip.Show(message, c, 5000);
            if (focus)
            {
                c.Focus();
            }
        }

        /// <summary>
        /// 显示一个气泡提示。
        /// </summary>
        /// <param name="c">显示所位于的控件。</param>
        /// <param name="message">信息。</param>
        /// <param name="during">显示所持续的时间。</param>
        public static void ShowToolTip(Control c, string message, int during)
        {
            ClearToolTip();
            toolTip.Show(message, c, during);
        }

        /// <summary>
        /// 显示一个气泡提示。
        /// </summary>
        /// <param name="c">显示所位于的控件。</param>
        /// <param name="message">信息。</param>
        /// <param name="location">气泡的位置。</param>
        public static void ShowToolTip(Control c, string message, Point location)
        {
            ClearToolTip();
            toolTip.Show(message, c, location, 5000);
        }

        /// <summary>
        /// 显示一个气泡提示。
        /// </summary>
        /// <param name="c">显示所位于的控件。</param>
        /// <param name="message">信息。</param>
        /// <param name="during">显示所持续的时间。</param>
        /// <param name="location">气泡的位置。</param>
        public static void ShowToolTip(Control c, string message, int during, Point location)
        {
            ClearToolTip();
            toolTip.Show(message, c, location, during);
        }
    }
}
