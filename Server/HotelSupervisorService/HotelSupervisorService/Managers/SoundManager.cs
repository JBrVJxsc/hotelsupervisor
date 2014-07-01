using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HotelSupervisorService.Managers
{
    public class SoundManager
    {
        private static bool playing = false;
        private string alertSountLocation = Application.StartupPath + "\\Sounds\\alert.wav";

        [DllImport("winmm")]
        public static extern bool PlaySound(string szSound, IntPtr hMod, int flags);

        public void PlayAlert()
        {
            if (playing)
            {
                return;
            }
            playing = true;
            if (!File.Exists(alertSountLocation))
            {
                return;
            }
            PlaySound(alertSountLocation, IntPtr.Zero, 0x00020000 | 0x0001);
            playing = false;
        }
    }
}
