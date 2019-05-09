using System;
using System.Runtime.InteropServices;

namespace SkinAssistance.Core.VBI.Presentation.Core
{
    [StructLayout(LayoutKind.Sequential)]
    public struct EXCEPTION_RECORD
    {
        /// <summary>
        /// 异常代码的指针地址
        /// </summary>
        public IntPtr ExceptionCode;

    }
}