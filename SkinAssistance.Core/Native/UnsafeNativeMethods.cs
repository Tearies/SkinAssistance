// ***********************************************************************
// Assembly         : VBI.Player.Core
// Author           : Tearies
// Created          : 09-28-2017
//
// Last Modified By : Tearies
// Last Modified On : 11-10-2017
// ***********************************************************************
// <copyright file="UnsafeNativeMethods.cs" company="北京恒泰实达科技股份有限公司">
//     Copyright (c) 北京恒泰实达科技股份有限公司. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

#region NS

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Accessibility;
using Microsoft.Win32.SafeHandles;
using SkinAssistance.Core.ApplicationService;

#endregion

namespace SkinAssistance.Core.Native
{
    #region Reference

    #endregion

    /// <summary>
    ///     Class UnsafeNativeMethods. This class cannot be inherited.
    /// </summary>
    [ComVisible(false)]
    [SuppressUnmanagedCodeSecurity]
    internal sealed class UnsafeNativeMethods
    {
        #region Method

        /// <summary>
        ///     Ints the delete object.
        /// </summary>
        /// <param name="hObject">The h object.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ExternDll.Gdi32, EntryPoint = "DeleteObject", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool IntDeleteObject(IntPtr hObject);

        /// <summary>
        ///     Deletes the object.
        /// </summary>
        /// <param name="hObject">The h object.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [SecurityCritical]
        public static bool DeleteObject(IntPtr hObject)
        {
            var result = IntDeleteObject(hObject);
            var error = Marshal.GetLastWin32Error();

            if (!result)
                Debug.WriteLine("DeleteObject failed.  Error = " + error);

            return result;
        }

        /// <summary>
        ///     Ints the try get cursor position.
        /// </summary>
        /// <param name="pt">The pt.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ExternDll.User32, EntryPoint = "GetCursorPos", ExactSpelling = true, CharSet = CharSet.Auto)]
        private static extern bool IntTryGetCursorPos([In] [Out] POINT pt);

        /// <summary>
        ///     Tries the get cursor position.
        /// </summary>
        /// <param name="pt">The pt.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [SecurityCritical]
        internal static bool TryGetCursorPos([In] [Out] POINT pt)
        {
            var returnValue = IntTryGetCursorPos(pt);

            // Sometimes Win32 will fail this call, such as if you are
            // not running in the interactive desktop.  For example,
            // a secure screen saver may be running.
            if (returnValue == false)
            {
                Debug.WriteLine("GetCursorPos failed!");

                pt.x = 0;
                pt.y = 0;
            }
            return returnValue;
        }

        /// <summary>
        ///     Gets the name of the class.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="lpClassName">Name of the lp class.</param>
        /// <param name="nMaxCount">The n maximum count.</param>
        /// <returns>System.Int32.</returns>
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        [DllImport(ExternDll.User32, SetLastError = true, CharSet = CharSet.Auto, BestFitMapping = false)]
        public static extern int GetClassName(HandleRef hwnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>
        ///     Gets the foreground window.
        /// </summary>
        /// <returns>IntPtr.</returns>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        ///     Gets the window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="uCmd">The u command.</param>
        /// <returns>IntPtr.</returns>
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetWindow(HandleRef hWnd, int uCmd);

        /// <summary>
        ///     Clients to screen.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="pt">The pt.</param>
        /// <exception cref="Win32Exception"></exception>
        [SecurityCritical]
        public static void ClientToScreen(HandleRef hWnd, [In] [Out] POINT pt)
        {
            if (IntClientToScreen(hWnd, pt) == 0)
                throw new Win32Exception();
        }

        /// <summary>
        ///     Ints the client to screen.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="pt">The pt.</param>
        /// <returns>System.Int32.</returns>
        /// <SecurityNote>
        ///     Critical - performs an elevation via SUC.
        /// </SecurityNote>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ExternDll.User32, EntryPoint = "ClientToScreen", SetLastError = true, ExactSpelling = true,
            CharSet = CharSet.Auto)]
        private static extern int IntClientToScreen(HandleRef hWnd, [In] [Out] POINT pt);

        /// <summary>
        ///     Objects from lresult.
        /// </summary>
        /// <param name="lResult">The l result.</param>
        /// <param name="iid">The iid.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="ppvObject">The PPV object.</param>
        /// <returns>System.Int32.</returns>
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        [DllImport("oleacc.dll")]
        internal static extern int ObjectFromLresult(IntPtr lResult, ref Guid iid, IntPtr wParam,
            [In] [Out] ref IAccessible ppvObject);

        /// <summary>
        ///     Gets the parent.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <returns>IntPtr.</returns>
        /// <exception cref="Win32Exception"></exception>
        [SecurityCritical]
        public static IntPtr GetParent(HandleRef hWnd)
        {
            var parent = NativeMethodsSetLastError.GetParent(hWnd);
            var lastWin32Error = Marshal.GetLastWin32Error();
            if (parent == IntPtr.Zero && lastWin32Error != 0)
                throw new Win32Exception(lastWin32Error);
            return parent;
        }

        /// <summary>
        ///     Determines whether [is win event hook installed] [the specified winevent].
        /// </summary>
        /// <param name="winevent">The winevent.</param>
        /// <returns><c>true</c> if [is win event hook installed] [the specified winevent]; otherwise, <c>false</c>.</returns>
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        [DllImport("user32.dll")]
        internal static extern bool IsWinEventHookInstalled(int winevent);

        /// <summary>
        ///     Sets the window position.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="hWndInsertAfter">The h WND insert after.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="cx">The cx.</param>
        /// <param name="cy">The cy.</param>
        /// <param name="flags">The flags.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [SuppressUnmanagedCodeSecurity]
        [SecurityCritical]
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetWindowPos(HandleRef hWnd, HandleRef hWndInsertAfter, int x, int y, int cx, int cy,
            int flags);

        /// <summary>
        ///     Shows the window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nCmdShow">The n command show.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        [DllImport(ExternDll.User32, ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(HandleRef hWnd, int nCmdShow);

        /// <summary>
        ///     Gets the window long.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nIndex">Index of the n.</param>
        /// <returns>System.Int32.</returns>
        [SecurityCritical]
        internal static int GetWindowLong(HandleRef hWnd, int nIndex)
        {
            var iResult = 0;
            var result = IntPtr.Zero;
            var error = 0;

            if (IntPtr.Size == 4)
            {
                // use GetWindowLong
                iResult = NativeMethodsSetLastError.GetWindowLong(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(iResult);
            }
            else
            {
                // use GetWindowLongPtr
                result = NativeMethodsSetLastError.GetWindowLongPtr(hWnd, nIndex);
                error = Marshal.GetLastWin32Error();
                iResult = NativeMethods.IntPtrToInt32(result);
            }

            if (result == IntPtr.Zero && error != 0)
                Debug.WriteLine("GetWindowLong failed.  Error = " + error);

            return iResult;
        }

        /// <summary>
        ///     Criticals the set window long.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nIndex">Index of the n.</param>
        /// <param name="dwNewLong">The dw new long.</param>
        /// <returns>IntPtr.</returns>
        internal static IntPtr CriticalSetWindowLong(HandleRef hWnd, int nIndex, IntPtr dwNewLong)
        {
            var result = IntPtr.Zero;

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                var tempResult =
                    NativeMethodsSetLastError.SetWindowLong(hWnd, nIndex, NativeMethods.IntPtrToInt32(dwNewLong));
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = NativeMethodsSetLastError.SetWindowLongPtr(hWnd, nIndex, dwNewLong);
            }

            return result;
        }


        /// <summary>
        ///     Prevents a default instance of the <see cref="UnsafeNativeMethods" /> class from being created.
        /// </summary>
        [SecurityCritical]
        private UnsafeNativeMethods()
        {
        }

        /// <summary>
        ///     Creates the file mapping.
        /// </summary>
        /// <param name="hFile">The h file.</param>
        /// <param name="lpAttributes">The lp attributes.</param>
        /// <param name="flProtect">The fl protect.</param>
        /// <param name="dwMaxSizeHi">The dw maximum size hi.</param>
        /// <param name="dwMaxSizeLow">The dw maximum size low.</param>
        /// <param name="lpName">Name of the lp.</param>
        /// <returns>SafeFileHandle.</returns>
        [SecurityCritical]
        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeFileHandle CreateFileMapping(HandleRef hFile,
            [MarshalAs(UnmanagedType.LPStruct)] NativeTypes.SECURITY_ATTRIBUTES lpAttributes, int flProtect,
            int dwMaxSizeHi, int dwMaxSizeLow, string lpName);


        /// <summary>
        ///     Locals the free.
        /// </summary>
        /// <param name="LocalHandle">The local handle.</param>
        /// <returns>IntPtr.</returns>
        [SecurityCritical]
        [DllImport(ExternDll.Kernel32, SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr LocalFree(IntPtr LocalHandle);

        /// <summary>
        ///     Maps the view of file.
        /// </summary>
        /// <param name="hFileMapping">The h file mapping.</param>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <param name="dwFileOffsetHigh">The dw file offset high.</param>
        /// <param name="dwFileOffsetLow">The dw file offset low.</param>
        /// <param name="dwNumberOfBytesToMap">The dw number of bytes to map.</param>
        /// <returns>SafeMemoryMappedViewOfFileHandle.</returns>
        [SecurityCritical]
        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeMemoryMappedViewOfFileHandle MapViewOfFile(IntPtr hFileMapping, int dwDesiredAccess,
            int dwFileOffsetHigh, int dwFileOffsetLow, UIntPtr dwNumberOfBytesToMap);


        /// <summary>
        ///     Opens the file mapping.
        /// </summary>
        /// <param name="dwDesiredAccess">The dw desired access.</param>
        /// <param name="bInheritHandle">if set to <c>true</c> [b inherit handle].</param>
        /// <param name="lpName">Name of the lp.</param>
        /// <returns>SafeFileHandle.</returns>
        [SecurityCritical]
        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern SafeFileHandle OpenFileMapping(int dwDesiredAccess,
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, string lpName);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool RedrawWindow(IntPtr hwnd,[In]ref RECT rcUpdate, IntPtr hrgnUpdate, RedrawWindowFlags flags);


        /// <summary>
        ///     Unmaps the view of file.
        /// </summary>
        /// <param name="pvBaseAddress">The pv base address.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [return: MarshalAs(UnmanagedType.Bool)]
        [SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [DllImport(ExternDll.Kernel32, CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern bool UnmapViewOfFile(IntPtr pvBaseAddress);

        /// <summary>
        ///     Determines whether the specified h WND is window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <returns><c>true</c> if the specified h WND is window; otherwise, <c>false</c>.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern bool IsWindow(IntPtr hWnd);

        /// <summary>
        ///     Gets the window rect.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="rect">The rect.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport(ExternDll.User32, EntryPoint = "GetWindowRect", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT rect);

        /// <summary>
        ///     Gets the dc.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.User32, EntryPoint = "GetDC", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        /// <summary>
        ///     Gets the desktop window.
        /// </summary>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.User32, EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();


        /// <summary>
        ///     Criticals the create compatible dc.
        /// </summary>
        /// <param name="hDC">The h dc.</param>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.Gdi32, EntryPoint = "CreateCompatibleDC", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CriticalCreateCompatibleDC(IntPtr hDC);

        /// <summary>
        ///     Criticals the create compatible bitmap.
        /// </summary>
        /// <param name="hDC">The h dc.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.Gdi32, EntryPoint = "CreateCompatibleBitmap", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CriticalCreateCompatibleBitmap(IntPtr hDC, int width, int height);

        /// <summary>
        ///     Selects the object.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        /// <param name="obj">The object.</param>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.Gdi32, EntryPoint = "SelectObject", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

        /// <summary>
        ///     Criticals the get stock object.
        /// </summary>
        /// <param name="stockObject">The stock object.</param>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.Gdi32, EntryPoint = "GetStockObject", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CriticalGetStockObject(int stockObject);

        /// <summary>
        ///     Criticals the fill rect.
        /// </summary>
        /// <param name="hdc">The HDC.</param>
        /// <param name="rcFill">The rc fill.</param>
        /// <param name="brush">The brush.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.User32, EntryPoint = "FillRect", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int CriticalFillRect(IntPtr hdc, ref RECT rcFill, IntPtr brush);

        /// <summary>
        ///     Prints the window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="hDC">The h dc.</param>
        /// <param name="flags">The flags.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport(ExternDll.User32, EntryPoint = "PrintWindow", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool PrintWindow(IntPtr hWnd, IntPtr hDC, int flags);

        /// <summary>
        ///     Sends the message.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>IntPtr.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        internal static extern IntPtr SendMessage(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        ///     Criticals the redraw window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="lprcUpdate">The LPRC update.</param>
        /// <param name="hrgnUpdate">The HRGN update.</param>
        /// <param name="flags">The flags.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport(ExternDll.User32, EntryPoint = "RedrawWindow", CharSet = CharSet.Auto)]
        public static extern bool CriticalRedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, int flags);

        /// <summary>
        ///     Releases the dc.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="hDC">The h dc.</param>
        /// <returns>System.Int32.</returns>
        [DllImport(ExternDll.User32, EntryPoint = "ReleaseDC", CharSet = CharSet.Auto)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);


        /// <summary>
        ///     Deletes the dc.
        /// </summary>
        /// <param name="hDC">The h dc.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport(ExternDll.Gdi32, EntryPoint = "DeleteDC", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool DeleteDC(IntPtr hDC);

        /// <summary>
        ///     Sets the window position.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="hWndInsertAfter">The h WND insert after.</param>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="cx">The cx.</param>
        /// <param name="cy">The cy.</param>
        /// <param name="flags">The flags.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy,
            int flags);

        /// <summary>
        ///     Shows the window.
        /// </summary>
        /// <param name="hWnd">The h WND.</param>
        /// <param name="nCmdShow">The n command show.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport(ExternDll.User32, CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        ///     The srccopy
        /// </summary>
        public const int SRCCOPY = 13369376;

        /// <summary>
        ///     Bits the BLT.
        /// </summary>
        /// <param name="hdcDest">The HDC dest.</param>
        /// <param name="xDest">The x dest.</param>
        /// <param name="yDest">The y dest.</param>
        /// <param name="wDest">The w dest.</param>
        /// <param name="hDest">The h dest.</param>
        /// <param name="hdcSource">The HDC source.</param>
        /// <param name="xSrc">The x source.</param>
        /// <param name="ySrc">The y source.</param>
        /// <param name="RasterOp">The raster op.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport(ExternDll.Gdi32, EntryPoint = "BitBlt")]
        public static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int wDest, int hDest, IntPtr hdcSource,
            int xSrc, int ySrc, int RasterOp);

        #endregion
    }

    [Flags()]
    public enum RedrawWindowFlags : uint
    {
        /// <summary>
        /// Invalidates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
        /// You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_INVALIDATE invalidates the entire window.
        /// </summary>
        Invalidate = 0x1,

        /// <summary>Causes the OS to post a WM_PAINT message to the window regardless of whether a portion of the window is invalid.</summary>
        InternalPaint = 0x2,

        /// <summary>
        /// Causes the window to receive a WM_ERASEBKGND message when the window is repainted.
        /// Specify this value in combination with the RDW_INVALIDATE value; otherwise, RDW_ERASE has no effect.
        /// </summary>
        Erase = 0x4,

        /// <summary>
        /// Validates the rectangle or region that you specify in lprcUpdate or hrgnUpdate.
        /// You can set only one of these parameters to a non-NULL value. If both are NULL, RDW_VALIDATE validates the entire window.
        /// This value does not affect internal WM_PAINT messages.
        /// </summary>
        Validate = 0x8,



        NoInternalPaint = 0x10,

        /// <summary>Suppresses any pending WM_ERASEBKGND messages.</summary>
        NoErase = 0x20,

        /// <summary>Excludes child windows, if any, from the repainting operation.</summary>
        NoChildren = 0x40,

        /// <summary>Includes child windows, if any, in the repainting operation.</summary>
        AllChildren = 0x80,

        /// <summary>Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive WM_ERASEBKGND and WM_PAINT messages before the RedrawWindow returns, if necessary.</summary>
        UpdateNow = 0x100,

        /// <summary>
        /// Causes the affected windows, which you specify by setting the RDW_ALLCHILDREN and RDW_NOCHILDREN values, to receive WM_ERASEBKGND messages before RedrawWindow returns, if necessary.
        /// The affected windows receive WM_PAINT messages at the ordinary time.
        /// </summary>
        EraseNow = 0x200,

        Frame = 0x400,

        NoFrame = 0x800
    }

}