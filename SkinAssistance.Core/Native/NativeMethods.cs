// ***********************************************************************
// Assembly         : VBI.Player.Core
// Author           : Tearies
// Created          : 11-10-2017
//
// Last Modified By : Tearies
// Last Modified On : 11-10-2017
// ***********************************************************************
// <copyright file="NativeMethods.cs" company="������̩ʵ��Ƽ��ɷ����޹�˾">
//     Copyright (c) ������̩ʵ��Ƽ��ɷ����޹�˾. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.ConstrainedExecution;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using SkinAssistance.Core.HotKey;

namespace SkinAssistance.Core.Native
{
    /// <summary>
    /// Class NativeMethods.
    /// </summary>
    [SuppressUnmanagedCodeSecurity]
    public class NativeMethods
    {
        #region Fields
        /// <summary>
        /// The vk shift
        /// </summary>
        public const byte VK_SHIFT = 0x10;
        /// <summary>
        /// The vk capital
        /// </summary>
        public const byte VK_CAPITAL = 0x14;
        /// <summary>
        /// The vk numlock
        /// </summary>
        public const byte VK_NUMLOCK = 0x90;
        /// <summary>
        /// The event object statechange
        /// </summary>
        public const int EVENT_OBJECT_STATECHANGE = 0x800A;

        /// <summary>
        /// The event object focus
        /// </summary>
        public const int EVENT_OBJECT_FOCUS = 0x8005;

        /// <summary>
        /// The objid client
        /// </summary>
        public const int OBJID_CLIENT = unchecked((int)0xFFFFFFFC);

        /// <summary>
        /// The s ok
        /// </summary>
        public const int S_OK = 0x00000000;

        /// <summary>
        /// The s false
        /// </summary>
        public const int S_FALSE = 0x00000001;

        /// <summary>
        /// The ma activate
        /// </summary>
        public const int MA_ACTIVATE = 0x0001;

        /// <summary>
        /// The abs notontop
        /// </summary>
        public const int ABS_NOTONTOP = 0;

        /// <summary>
        /// The abs autohide
        /// </summary>
        public const int ABS_AUTOHIDE = 1;

        /// <summary>
        /// The abs ontop
        /// </summary>
        public const int ABS_ONTOP = 2;

        /// <summary>
        /// The abm setstate
        /// </summary>
        public const int ABM_SETSTATE = 10;

        /// <summary>
        /// The maximum path
        /// </summary>
        public const int MAX_PATH = 260;

        /// <summary>
        /// The gw hwndfirst
        /// </summary>
        public const int GW_HWNDFIRST = 0;

        /// <summary>
        /// The gw hwndlast
        /// </summary>
        public const int GW_HWNDLAST = 1;

        /// <summary>
        /// The gw hwndnext
        /// </summary>
        public const int GW_HWNDNEXT = 2;

        /// <summary>
        /// The gw hwndprev
        /// </summary>
        public const int GW_HWNDPREV = 3;

        /// <summary>
        /// The GWL wndproc
        /// </summary>
        public const int GWL_WNDPROC = -4;

        /// <summary>
        /// The GWL hwndparent
        /// </summary>
        public const int GWL_HWNDPARENT = -8;

        /// <summary>
        /// The GWL style
        /// </summary>
        public const int GWL_STYLE = -16;

        /// <summary>
        /// The GWL exstyle
        /// </summary>
        public const int GWL_EXSTYLE = -20;

        /// <summary>
        /// The GWL identifier
        /// </summary>
        public const int GWL_ID = -12;

        /// <summary>
        /// The ma activateandeat
        /// </summary>
        public const int MA_ACTIVATEANDEAT = 0x0002;

        /// <summary>
        /// The ma noactivate
        /// </summary>
        public const int MA_NOACTIVATE = 0x0003;

        /// <summary>
        /// The ma noactivateandeat
        /// </summary>
        public const int MA_NOACTIVATEANDEAT = 0x0004;

        /// <summary>
        /// The sw hide
        /// </summary>
        public const int SW_HIDE = 0;

        /// <summary>
        /// The sw normal
        /// </summary>
        public const int SW_NORMAL = 1;

        /// <summary>
        /// The sw showminimized
        /// </summary>
        public const int SW_SHOWMINIMIZED = 2;

        /// <summary>
        /// The sw showmaximized
        /// </summary>
        public const int SW_SHOWMAXIMIZED = 3;

        /// <summary>
        /// The sw maximize
        /// </summary>
        public const int SW_MAXIMIZE = 3;

        /// <summary>
        /// The sw shownoactivate
        /// </summary>
        public const int SW_SHOWNOACTIVATE = 4;

        /// <summary>
        /// The sw show
        /// </summary>
        public const int SW_SHOW = 5;

        /// <summary>
        /// The sw minimize
        /// </summary>
        public const int SW_MINIMIZE = 6;

        /// <summary>
        /// The sw showminnoactive
        /// </summary>
        public const int SW_SHOWMINNOACTIVE = 7;

        /// <summary>
        /// The sw showna
        /// </summary>
        public const int SW_SHOWNA = 8;

        /// <summary>
        /// The sw restore
        /// </summary>
        public const int SW_RESTORE = 9;

        /// <summary>
        /// The sw maximum
        /// </summary>
        public const int SW_MAX = 10;

        /// <summary>
        /// The SWP nosize
        /// </summary>
        public const int SWP_NOSIZE = 0x0001;

        /// <summary>
        /// The SWP nomove
        /// </summary>
        public const int SWP_NOMOVE = 0x0002;

        /// <summary>
        /// The SWP nozorder
        /// </summary>
        public const int SWP_NOZORDER = 0x0004;

        /// <summary>
        /// The SWP noactivate
        /// </summary>
        public const int SWP_NOACTIVATE = 0x0010;

        /// <summary>
        /// The SWP showwindow
        /// </summary>
        public const int SWP_SHOWWINDOW = 0x0040;

        /// <summary>
        /// The SWP hidewindow
        /// </summary>
        public const int SWP_HIDEWINDOW = 0x0080;

        /// <summary>
        /// The SWP noredraw
        /// </summary>
        public const int SWP_NOREDRAW = 0x0008;

        /// <summary>
        /// The SWP framechanged
        /// </summary>
        public const int SWP_FRAMECHANGED = 0x0020; // The frame changed: send WM_NCCALCSIZE

        /// <summary>
        /// The SWP nocopybits
        /// </summary>
        public const int SWP_NOCOPYBITS = 0x0100;

        /// <summary>
        /// The SWP noownerzorder
        /// </summary>
        public const int SWP_NOOWNERZORDER = 0x0200; // Don't do owner Z ordering

        /// <summary>
        /// The SWP nosendchanging
        /// </summary>
        public const int SWP_NOSENDCHANGING = 0x0400; // Don't send WM_WINDOWPOSCHANGING

        /// <summary>
        /// The SWP noreposition
        /// </summary>
        public const int SWP_NOREPOSITION = SWP_NOOWNERZORDER;

        /// <summary>
        /// The SWP defererase
        /// </summary>
        public const int SWP_DEFERERASE = 0x2000;

        /// <summary>
        /// The SWP asyncwindowpos
        /// </summary>
        public const int SWP_ASYNCWINDOWPOS = 0x4000;

        /// <summary>
        /// The SWP drawframe
        /// </summary>
        public const int SWP_DRAWFRAME = 0x0020;

        /// <summary>
        /// The ws overlapped
        /// </summary>
        public const int WS_OVERLAPPED = 0x00000000;

        /// <summary>
        /// The ws popup
        /// </summary>
        public const int WS_POPUP = unchecked((int)0x80000000);

        /// <summary>
        /// The ws child
        /// </summary>
        public const int WS_CHILD = 0x40000000;

        /// <summary>
        /// The ws minimize
        /// </summary>
        public const int WS_MINIMIZE = 0x20000000;

        /// <summary>
        /// The ws visible
        /// </summary>
        public const int WS_VISIBLE = 0x10000000;

        /// <summary>
        /// The ws disabled
        /// </summary>
        public const int WS_DISABLED = 0x08000000;

        /// <summary>
        /// The ws clipsiblings
        /// </summary>
        public const int WS_CLIPSIBLINGS = 0x04000000;

        /// <summary>
        /// The ws clipchildren
        /// </summary>
        public const int WS_CLIPCHILDREN = 0x02000000;

        /// <summary>
        /// The ws maximize
        /// </summary>
        public const int WS_MAXIMIZE = 0x01000000;

        /// <summary>
        /// The ws caption
        /// </summary>
        public const int WS_CAPTION = 0x00C00000;

        /// <summary>
        /// The ws border
        /// </summary>
        public const int WS_BORDER = 0x00800000;

        /// <summary>
        /// The ws dlgframe
        /// </summary>
        public const int WS_DLGFRAME = 0x00400000;

        /// <summary>
        /// The ws vscroll
        /// </summary>
        public const int WS_VSCROLL = 0x00200000;

        /// <summary>
        /// The ws hscroll
        /// </summary>
        public const int WS_HSCROLL = 0x00100000;

        /// <summary>
        /// The ws sysmenu
        /// </summary>
        public const int WS_SYSMENU = 0x00080000;

        /// <summary>
        /// The ws thickframe
        /// </summary>
        public const int WS_THICKFRAME = 0x00040000;

        /// <summary>
        /// The ws tabstop
        /// </summary>
        public const int WS_TABSTOP = 0x00010000;

        /// <summary>
        /// The ws minimizebox
        /// </summary>
        public const int WS_MINIMIZEBOX = 0x00020000;

        /// <summary>
        /// The ws maximizebox
        /// </summary>
        public const int WS_MAXIMIZEBOX = 0x00010000;

        /// <summary>
        /// The ws ex dlgmodalframe
        /// </summary>
        public const int WS_EX_DLGMODALFRAME = 0x00000001;

        /// <summary>
        /// The ws ex transparent
        /// </summary>
        public const int WS_EX_TRANSPARENT = 0x00000020;

        /// <summary>
        /// The ws ex mdichild
        /// </summary>
        public const int WS_EX_MDICHILD = 0x00000040;

        /// <summary>
        /// The ws ex toolwindow
        /// </summary>
        public const int WS_EX_TOOLWINDOW = 0x00000080;

        /// <summary>
        /// The ws ex windowedge
        /// </summary>
        public const int WS_EX_WINDOWEDGE = 0x00000100;

        /// <summary>
        /// The ws ex clientedge
        /// </summary>
        public const int WS_EX_CLIENTEDGE = 0x00000200;

        /// <summary>
        /// The ws ex contexthelp
        /// </summary>
        public const int WS_EX_CONTEXTHELP = 0x00000400;

        /// <summary>
        /// The ws ex right
        /// </summary>
        public const int WS_EX_RIGHT = 0x00001000;

        /// <summary>
        /// The ws ex left
        /// </summary>
        public const int WS_EX_LEFT = 0x00000000;

        /// <summary>
        /// The ws ex rtlreading
        /// </summary>
        public const int WS_EX_RTLREADING = 0x00002000;

        /// <summary>
        /// The ws ex leftscrollbar
        /// </summary>
        public const int WS_EX_LEFTSCROLLBAR = 0x00004000;

        /// <summary>
        /// The ws ex controlparent
        /// </summary>
        public const int WS_EX_CONTROLPARENT = 0x00010000;

        /// <summary>
        /// The ws ex staticedge
        /// </summary>
        public const int WS_EX_STATICEDGE = 0x00020000;

        /// <summary>
        /// The ws ex appwindow
        /// </summary>
        public const int WS_EX_APPWINDOW = 0x00040000;

        /// <summary>
        /// The ws ex layered
        /// </summary>
        public const int WS_EX_LAYERED = 0x00080000;

        /// <summary>
        /// The ws ex topmost
        /// </summary>
        public const int WS_EX_TOPMOST = 0x00000008;

        /// <summary>
        /// The ws ex layoutrtl
        /// </summary>
        public const int WS_EX_LAYOUTRTL = 0x00400000;

        /// <summary>
        /// The ws ex noinheritlayout
        /// </summary>
        public const int WS_EX_NOINHERITLAYOUT = 0x00100000;

        /// <summary>
        /// The ws ex composited
        /// </summary>
        public const int WS_EX_COMPOSITED = 0x02000000;

        /// <summary>
        /// The WPF setminposition
        /// </summary>
        public const int WPF_SETMINPOSITION = 0x0001;

        /// <summary>
        /// The ws ex noactivate
        /// </summary>
        public const int WS_EX_NOACTIVATE = 0x08000000;

        /// <summary>
        /// The WPF restoretomaximized
        /// </summary>
        public const int WPF_RESTORETOMAXIMIZED = 0x0002;

        /// <summary>
        /// The wh journalplayback
        /// </summary>
        public const int WH_JOURNALPLAYBACK = 1;

        /// <summary>
        /// The wh getmessage
        /// </summary>
        public const int WH_GETMESSAGE = 3;

        /// <summary>
        /// The wh mouse ll
        /// </summary>
        public const int WH_MOUSE_LL = 14;
        /// <summary>
        /// The wh keyboard ll
        /// </summary>
        public const int WH_KEYBOARD_LL = 13;
        /// <summary>
        /// The wh mouse
        /// </summary>
        public const int WH_MOUSE = 7;
        /// <summary>
        /// The wh keyboard
        /// </summary>
        public const int WH_KEYBOARD = 2;

        /// <summary>
        /// The WSF visible
        /// </summary>
        public const int WSF_VISIBLE = 0x0001;

        /// <summary>
        /// The wa inactive
        /// </summary>
        public const int WA_INACTIVE = 0;

        /// <summary>
        /// The wa active
        /// </summary>
        public const int WA_ACTIVE = 1;

        /// <summary>
        /// The wa clickactive
        /// </summary>
        public const int WA_CLICKACTIVE = 2;


        /// <summary>
        /// The null handle reference
        /// </summary>
        public static HandleRef NullHandleRef = new HandleRef(null, IntPtr.Zero);

        /// <summary>
        /// The HWND top
        /// </summary>
        public static HandleRef HWND_TOP = new HandleRef(null, (IntPtr)0);

        /// <summary>
        /// The HWND bottom
        /// </summary>
        public static HandleRef HWND_BOTTOM = new HandleRef(null, (IntPtr)1);

        /// <summary>
        /// The HWND topmost
        /// </summary>
        public static HandleRef HWND_TOPMOST = new HandleRef(null, new IntPtr(-1));

        /// <summary>
        /// The HWND notopmost
        /// </summary>
        public static HandleRef HWND_NOTOPMOST = new HandleRef(null, new IntPtr(-2));

        #endregion

        #region Members
        [DllImport("kernel32.dll")]
         
        public static extern int GlobalAddAtom(String lpString);

        [DllImport("kernel32.dll")]

        public static extern int GlobalDeleteAtom(int nAtom);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, HotkeyFlags fsModifiers, uint vk);

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        /// <summary>
        /// Sets the windows hook ex.
        /// </summary>
        /// <param name="idHook">The identifier hook.</param>
        /// <param name="lpfn">The LPFN.</param>
        /// <param name="hMod">The h mod.</param>
        /// <param name="dwThreadId">The dw thread identifier.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, int dwThreadId);

        /// <summary>
        /// Unhooks the windows hook ex.
        /// </summary>
        /// <param name="idHook">The identifier hook.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        public static extern int UnhookWindowsHookEx(int idHook);

        /// <summary>
        /// Calls the next hook ex.
        /// </summary>
        /// <param name="idHook">The identifier hook.</param>
        /// <param name="nCode">The n code.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

        /// <summary>
        /// Delegate HookProc
        /// </summary>
        /// <param name="nCode">The n code.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>System.Int32.</returns>
        public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

        /// <summary>
        /// To the ASCII.
        /// </summary>
        /// <param name="uVirtKey">The u virt key.</param>
        /// <param name="uScanCode">The u scan code.</param>
        /// <param name="lpbKeyState">State of the LPB key.</param>
        /// <param name="lpwTransKey">The LPW trans key.</param>
        /// <param name="fuState">State of the fu.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.User32)]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        /// <summary>
        /// Gets the state of the keyboard.
        /// </summary>
        /// <param name="pbKeyState">State of the pb key.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.User32)]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        /// <summary>
        /// Gets the state of the key.
        /// </summary>
        /// <param name="vKey">The v key.</param>
        /// <returns>System.Int16.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int vKey);

        /// <summary>
        /// Gets the module handle a.
        /// </summary>
        /// <param name="lpModuleName">Name of the lp module.</param>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.Kernel32, EntryPoint = "GetModuleHandleA", SetLastError = true, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true)]
        public static extern IntPtr GetModuleHandleA(String lpModuleName);

        /// <summary>
        /// Gets the module handle w.
        /// </summary>
        /// <param name="lpModuleName">Name of the lp module.</param>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.Kernel32, EntryPoint = "GetModuleHandleW", SetLastError = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        public static extern IntPtr GetModuleHandleW(String lpModuleName);
        /// <summary>
        /// Finds the window a.
        /// </summary>
        /// <param name="lp1">The LP1.</param>
        /// <param name="lp2">The LP2.</param>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.User32, EntryPoint = "FindWindowA")]
        public static extern IntPtr FindWindowA(string lp1, string lp2);

        /// <summary>
        /// Shes the application bar message.
        /// </summary>
        /// <param name="dwMessage">The dw message.</param>
        /// <param name="pData">The p data.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.Shell32)]
        public static extern int SHAppBarMessage(int dwMessage, ref Appbardata pData);

        /// <summary>
        /// Multis the monitor support.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool MultiMonitorSupport()
        {
            return GetSystemMetrics(80) > 0u;
        }

        /// <summary>
        /// Ints the PTR to int32.
        /// </summary>
        /// <param name="intPtr">The int PTR.</param>
        /// <returns>System.Int32.</returns>
        public static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        /// <summary>
        /// Gets the capture.
        /// </summary>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetCapture();

 

        /// <summary>
        /// Frees the library.
        /// </summary>
        /// <param name="hModule">The h module.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(ExternDll.Kernel32, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeLibrary(IntPtr hModule);

  

        /// <summary>
        /// Gets the system metrics.
        /// </summary>
        /// <param name="nIndex">Index of the n.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        private static extern int GetSystemMetrics(int nIndex);

        /// <summary>
        /// Closes the handle.
        /// </summary>
        /// <param name="hObject">The h object.</param>
        /// <returns>System.Int32.</returns>
        [SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int CloseHandle(IntPtr hObject);

        /// <summary>
        /// Converts the string security descriptor to security descriptor.
        /// </summary>
        /// <param name="StringSecurityDescriptor">The string security descriptor.</param>
        /// <param name="StringSDRevision">The string sd revision.</param>
        /// <param name="SecurityDescriptor">The security descriptor.</param>
        /// <param name="SecurityDescriptorSize">Size of the security descriptor.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [SecurityCritical]
        [DllImport(ExternDll.Advapi32, CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool ConvertStringSecurityDescriptorToSecurityDescriptor(string StringSecurityDescriptor,
            uint StringSDRevision, ref IntPtr SecurityDescriptor, IntPtr SecurityDescriptorSize);


        /// <summary>
        /// Sets the focus.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <returns>IntPtr.</returns>
        [SecurityCritical]
        [DllImport(ExternDll.User32, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr SetFocus(IntPtr hwnd);

        /// <summary>
        /// Sets the foreground window.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [SecurityCritical]
        [DllImport(ExternDll.User32, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(IntPtr hwnd);


        /// <summary>
        /// Gets the current thread identifier.
        /// </summary>
        /// <returns>System.UInt32.</returns>
        [DllImport(ExternDll.Kernel32)]
        public static extern uint GetCurrentThreadId();


        /// <summary>
        /// Attaches the thread input.
        /// </summary>
        /// <param name="idAttach">The identifier attach.</param>
        /// <param name="idAttachTo">The identifier attach to.</param>
        /// <param name="fAttach">if set to <c>true</c> [f attach].</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int AttachThreadInput(int idAttach, int idAttachTo, bool fAttach);

        /// <summary>
        /// Gets the foreground window.
        /// </summary>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();


        /// <summary>
        /// Shows the window.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="nCmdShow">The n command show.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [DllImport(ExternDll.User32)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hwnd, SW nCmdShow);


        /// <summary>
        /// Gets the window thread process identifier.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="ID">The identifier.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        #endregion        
        /// <summary>Criticals the redraw window.</summary>
        [HandleProcessCorruptedStateExceptions]
        public static void CriticalRedrawWindow()
        {
            try
            {
                var hwnd = ApplicationInfo.ApplicationService.MainWindow;
                RECT rect1 = new RECT();
                if (UnsafeNativeMethods.GetWindowRect(hwnd, ref rect1))
                {
                    UnsafeNativeMethods.RedrawWindow(hwnd, ref rect1, IntPtr.Zero,
                        RedrawWindowFlags.Frame | RedrawWindowFlags.UpdateNow | RedrawWindowFlags.Invalidate);
                }
            }
            catch
            {

            }
        }
    }
}