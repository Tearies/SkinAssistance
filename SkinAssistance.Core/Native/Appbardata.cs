using System;

namespace SkinAssistance.Core.Native
{
    public struct Appbardata
    {
        public int cbSize;
        public IntPtr hwnd;
        public int uCallbackMessage;
        public int uEdge;
        public RECT rc;
        public int lParam;
    }
}