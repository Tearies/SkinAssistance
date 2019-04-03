using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace SkinAssistance.Core.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public class ICONINFO
    {
        public bool fIcon = false;
        public int xHotspot = 0;
        public int yHotspot = 0;
        public SafeHandleZeroOrMinusOneIsInvalid hbmMask = null;
        public SafeHandleZeroOrMinusOneIsInvalid hbmColor = null;
    }
}