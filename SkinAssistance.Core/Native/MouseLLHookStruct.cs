using System.Runtime.InteropServices;

namespace SkinAssistance.Core.Native
{
    [StructLayout(LayoutKind.Sequential)]
    public class MouseLLHookStruct
    {
        public POINT pt;
        public int mouseData;
        public int flags;
        public int time;
        public int dwExtraInfo;
    }
}