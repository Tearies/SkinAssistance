using System.Runtime.InteropServices;

namespace SkinAssistance.Core.VBI.Presentation.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EXCEPTION_POINTERS
    {
        public EXCEPTION_RECORD ExceptionRecord;
    }
}