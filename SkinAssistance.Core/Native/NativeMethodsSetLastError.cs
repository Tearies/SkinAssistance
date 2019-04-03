using System;
using System.Runtime.InteropServices;

namespace SkinAssistance.Core.Native
{
    public class NativeMethodsSetLastError
    {
        #region Members

        [DllImport(ExternDll.PresentationNativeDll, EntryPoint = "GetParentWrapper", SetLastError = true)]
        public static extern IntPtr GetParent(HandleRef hWnd);

        [DllImport(ExternDll.PresentationNativeDll, EntryPoint = "GetWindowLongPtrWrapper", CharSet = CharSet.Auto,
            SetLastError = true)]
        public static extern IntPtr GetWindowLongPtr(HandleRef hWnd, int nIndex);

        [DllImport(ExternDll.PresentationNativeDll, EntryPoint = "GetWindowLongWrapper", CharSet = CharSet.Auto,
            SetLastError = true)]
        public static extern int GetWindowLong(HandleRef hWnd, int nIndex);

        [DllImport(ExternDll.PresentationNativeDll, EntryPoint = "SetWindowLongWrapper", CharSet = CharSet.Auto)]
        public static extern int SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport(ExternDll.PresentationNativeDll, EntryPoint = "SetWindowLongPtrWrapper", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLongPtr(HandleRef hWnd, int nIndex, IntPtr dwNewLong);

        #endregion
    }
}