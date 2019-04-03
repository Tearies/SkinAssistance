using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security;

namespace SkinAssistance.Core.Native
{
    internal static class SafeNativeMethods
    {
        #region Types

        [SuppressUnmanagedCodeSecurity, SecurityCritical(SecurityCriticalScope.Everything)]
        private class SafeNativeMethodsPrivate
        {
            #region Members

            [DllImport(ExternDll.Kernel32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern int GetCurrentProcessId();

            [DllImport(ExternDll.Kernel32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern int GetCurrentThreadId();

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern IntPtr GetCapture();

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern bool IsWindowVisible(HandleRef hWnd);

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern int GetMessagePos();

            [DllImport(ExternDll.User32, EntryPoint = "ReleaseCapture", ExactSpelling = true, CharSet = CharSet.Auto,
                SetLastError = true)]
            public static extern bool IntReleaseCapture();

            [DllImport(ExternDll.User32, EntryPoint = "GetWindowRect", ExactSpelling = true, CharSet = CharSet.Auto,
                SetLastError = true)]
            public static extern bool IntGetWindowRect(HandleRef hWnd, [In, Out] ref RECT rect);

            [DllImport(ExternDll.User32, EntryPoint = "GetClientRect", ExactSpelling = true, CharSet = CharSet.Auto,
                SetLastError = true)]
            public static extern bool IntGetClientRect(HandleRef hWnd, [In, Out] ref RECT rect);

            [DllImport(ExternDll.User32, EntryPoint = "AdjustWindowRectEx", ExactSpelling = true,
                CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool IntAdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

            [DllImport(ExternDll.User32, ExactSpelling = true)]
            public static extern IntPtr MonitorFromRect(ref RECT rect, int flags);

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern IntPtr ActivateKeyboardLayout(HandleRef hkl, int uFlags);

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern IntPtr GetKeyboardLayout(int dwLayout);

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern bool KillTimer(HandleRef hwnd, int idEvent);

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern bool IsWindowUnicode(HandleRef hWnd);

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern int GetDoubleClickTime();

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern bool IsWindowEnabled(HandleRef hWnd);

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern IntPtr GetCursor();

            [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
            public static extern int ShowCursor(bool show);

            [DllImport(ExternDll.User32, SetLastError = true, ExactSpelling = true)]
            public static extern IntPtr MonitorFromWindow(HandleRef handle, int flags);

            [DllImport(ExternDll.User32, EntryPoint = "ScreenToClient", SetLastError = true, ExactSpelling = true,
                CharSet = CharSet.Auto)]
            public static extern int IntScreenToClient(HandleRef hWnd, [In, Out] POINT pt);

            #endregion
        }

        #endregion

        #region Members

        /// <SecurityNote>
        ///     Critical: This code calls into unmanaged code which elevates
        ///     TreatAsSafe: This method is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static int GetMessagePos()
        {
            return SafeNativeMethodsPrivate.GetMessagePos();
        }

        /// <SecurityNote>
        ///     Critical: This code calls into unmanaged code which elevates
        ///     TreatAsSafe: This method is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static IntPtr GetKeyboardLayout(int dwLayout)
        {
            return SafeNativeMethodsPrivate.GetKeyboardLayout(dwLayout);
        }

        /// <SecurityNote>
        ///     Critical: This code calls into unmanaged code which elevates
        ///     TreatAsSafe: This method is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static IntPtr ActivateKeyboardLayout(HandleRef hkl, int uFlags)
        {
            return SafeNativeMethodsPrivate.ActivateKeyboardLayout(hkl, uFlags);
        }


        /// <SecurityNote>
        ///     Critical: This code calls into unmanaged code which elevates
        ///     TreatAsSafe: This method is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static IntPtr GetCursor()
        {
            return SafeNativeMethodsPrivate.GetCursor();
        }

        /// <SecurityNote>
        ///     Critical: This code elevates to unmanaged code permission
        ///     TreatAsSafe: Hiding cursor is ok
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static int ShowCursor(bool show)
        {
            return SafeNativeMethodsPrivate.ShowCursor(show);
        }

        /// <SecurityNote>
        ///     Critical: This code calls into unmanaged code which elevates
        ///     TreatAsSafe: This method is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        internal static bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle)
        {
            var returnValue = SafeNativeMethodsPrivate.IntAdjustWindowRectEx(ref lpRect, dwStyle, bMenu, dwExStyle);
            if (returnValue == false)
                throw new Win32Exception();
            return returnValue;
        }


        /// <SecurityNote>
        ///     Critical: This code calls into unmanaged code which elevates
        ///     TreatAsSafe: This method is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        internal static void GetClientRect(HandleRef hWnd, [In, Out] ref RECT rect)
        {
            if (!SafeNativeMethodsPrivate.IntGetClientRect(hWnd, ref rect))
                throw new Win32Exception();
        }

        /// <SecurityNote>
        ///     Critical: This code calls into unmanaged code which elevates
        ///     TreatAsSafe: This method is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        internal static void GetWindowRect(HandleRef hWnd, [In, Out] ref RECT rect)
        {
            if (!SafeNativeMethodsPrivate.IntGetWindowRect(hWnd, ref rect))
                throw new Win32Exception();
        }

        [SecurityCritical, SecuritySafeCritical]
        internal static int GetWindowStyle(HandleRef hWnd, bool exStyle)
        {
            var nIndex = exStyle ? NativeMethods.GWL_EXSTYLE : NativeMethods.GWL_STYLE;
            return UnsafeNativeMethods.GetWindowLong(hWnd, nIndex);
        }

        /// <SecurityNote>
        ///     Critical: This code elevates to unmanaged code permission
        ///     TreatAsafe: This function is safe to call
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static int GetDoubleClickTime()
        {
            return SafeNativeMethodsPrivate.GetDoubleClickTime();
        }

        /// <SecurityNote>
        ///     Critical: This code elevates to unmanaged code permission
        ///     TreatAsafe: This function is safe to call
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static bool IsWindowEnabled(HandleRef hWnd)
        {
            return SafeNativeMethodsPrivate.IsWindowEnabled(hWnd);
        }

        /// <SecurityNote>
        ///     Critical: This code elevates to unmanaged code permission
        ///     TreatAsafe: This function is safe to call
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static bool IsWindowVisible(HandleRef hWnd)
        {
            return SafeNativeMethodsPrivate.IsWindowVisible(hWnd);
        }

        /// <SecurityNote>
        ///     Critical: This code calls into unmanaged code which elevates
        ///     TreatAsSafe: This method is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        internal static bool ReleaseCapture()
        {
            var returnValue = SafeNativeMethodsPrivate.IntReleaseCapture();

            if (returnValue == false)
                throw new Win32Exception();
            return returnValue;
        }


        /// <SecurityNote>
        ///     Critical: This code elevates to unmanaged code permission
        ///     TreatAsafe: This function is safe to call as in the worst case it destroys the dispatcher timer.
        ///     it destroys a timer
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static bool KillTimer(HandleRef hwnd, int idEvent)
        {
            return SafeNativeMethodsPrivate.KillTimer(hwnd, idEvent);
        }


        /// <SecurityNote>
        ///     Critical: This code elevates to unmanaged code permission
        ///     TreatAsafe: This function is safe to call
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static bool IsWindowUnicode(HandleRef hWnd)
        {
            return SafeNativeMethodsPrivate.IsWindowUnicode(hWnd);
        }

        // not used by compiler - don't include.

        /// <SecurityNote>
        ///     Critical: This code elevates to unmanaged code permission
        ///     TreatAsSafe: Screen to Clien is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static void ScreenToClient(HandleRef hWnd, [In, Out] POINT pt)
        {
            if (SafeNativeMethodsPrivate.IntScreenToClient(hWnd, pt) == 0)
                throw new Win32Exception();
        }

        /// <SecurityNote>
        ///     Critical: This code elevates to unmanaged code permission
        ///     TreatAsSafe: Process Id is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static int GetCurrentProcessId()
        {
            return SafeNativeMethodsPrivate.GetCurrentProcessId();
        }


        /// <SecurityNote>
        ///     Critical: This code elevates to unmanaged code permission
        ///     TreatAsSafe: Thread ID is ok to give out
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static int GetCurrentThreadId()
        {
            return SafeNativeMethodsPrivate.GetCurrentThreadId();
        }

        /// <SecurityNote>
        ///     This will return a valid handle only if a window on the current thread has capture
        ///     else it will return NULL. (Refer to Platform SDK)
        ///     Critical: This code elevates to unmanaged code permission
        ///     TreatAsSafe: Getting mouse capture is ok
        /// </SecurityNote>
        [SecurityCritical, SecuritySafeCritical]
        public static IntPtr GetCapture()
        {
            return SafeNativeMethodsPrivate.GetCapture();
        }

        #endregion
    }
}