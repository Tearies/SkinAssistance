// ***********************************************************************
// Assembly         : VBI.Player.Core
// Author           : Tearies
// Created          : 09-28-2017
//
// Last Modified By : Tearies
// Last Modified On : 11-10-2017
// ***********************************************************************
// <copyright file="MouseKeyHook.cs" company="北京恒泰实达科技股份有限公司">
//     Copyright (c) 北京恒泰实达科技股份有限公司. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SkinAssistance.Core.Native
{
    /// <summary>
    ///     Class MouseKeyHook.
    /// </summary>
    public class MouseKeyHook
    {
        #region Fields

        /// <summary>
        ///     The mouse hook procedure
        /// </summary>
        private static NativeMethods.HookProc MouseHookProcedure;

        /// <summary>
        ///     The keyboard hook procedure
        /// </summary>
        private static NativeMethods.HookProc KeyboardHookProcedure;

        /// <summary>
        ///     The h keyboard hook
        /// </summary>
        private int hKeyboardHook;

        /// <summary>
        ///     The h mouse hook
        /// </summary>
        private int hMouseHook; //标记mouse hook是否安装  

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MouseKeyHook" /> class.
        /// </summary>
        public MouseKeyHook()
        {
            Start();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MouseKeyHook" /> class.
        /// </summary>
        /// <param name="InstallMouseHook">if set to <c>true</c> [install mouse hook].</param>
        /// <param name="InstallKeyboardHook">if set to <c>true</c> [install keyboard hook].</param>
        public MouseKeyHook(bool InstallMouseHook, bool InstallKeyboardHook)
        {
            Start(InstallMouseHook, InstallKeyboardHook);
        }

        #endregion

        #region Members

        /// <summary>
        ///     Finalizes an instance of the <see cref="MouseKeyHook" /> class.
        /// </summary>
        ~MouseKeyHook()
        {
            Stop(true, false, false);
        }

        /// <summary>
        ///     Occurs when [on mouse activity].
        /// </summary>
        public event MouseEventHandler OnMouseActivity
            ; //MouseEventHandler是委托，表示处理窗体、控件或其他组件的 MouseDown、MouseUp 或 MouseMove 事件的方法。  

        /// <summary>
        ///     Occurs when [key down].
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        ///     Occurs when [key press].
        /// </summary>
        public event KeyPressEventHandler KeyPress;

        /// <summary>
        ///     Occurs when [key up].
        /// </summary>
        public event KeyEventHandler KeyUp;

        //---------------------------------------------------------------------------  
        /// <summary>
        ///     Starts this instance.
        /// </summary>
        public void Start()
        {
            Start(true, false);
        }

        /// <summary>
        ///     Starts the specified install mouse hook.
        /// </summary>
        /// <param name="InstallMouseHook">if set to <c>true</c> [install mouse hook].</param>
        /// <param name="InstallKeyboardHook">if set to <c>true</c> [install keyboard hook].</param>
        /// <exception cref="Win32Exception">
        /// </exception>
        public void Start(bool InstallMouseHook, bool InstallKeyboardHook)
        {
            var HM = Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]);

            if (hMouseHook == 0 && InstallMouseHook)
            {
                MouseHookProcedure = MouseHookProc; //钩子的处理函数  
                hMouseHook = NativeMethods.SetWindowsHookEx(
                    NativeMethods.WH_MOUSE_LL,
                    MouseHookProcedure,
                    NativeMethods.GetModuleHandleW(Process.GetCurrentProcess().MainModule.ModuleName), //本进程模块句柄  
                    0);
                if (hMouseHook == 0)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    Stop(true, false, false);
                    throw new Win32Exception(errorCode);
                }
            }

            if (hKeyboardHook == 0 && InstallKeyboardHook)
            {
                KeyboardHookProcedure = KeyboardHookProc;
                hKeyboardHook = NativeMethods.SetWindowsHookEx(
                    NativeMethods.WH_KEYBOARD_LL,
                    KeyboardHookProcedure,
                    //Marshal.GetHINSTANCE( Assembly.GetExecutingAssembly().GetModules()[0]),  
                    NativeMethods.GetModuleHandleW(Process.GetCurrentProcess().MainModule.ModuleName),
                    0);
                if (hKeyboardHook == 0)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    Stop(false, true, false);
                    throw new Win32Exception(errorCode);
                }
            }
        }

        //-------------------------------------------------  
        /// <summary>
        ///     Stops this instance.
        /// </summary>
        public void Stop()
        {
            Stop(true, true, true);
        }

        /// <summary>
        ///     Stops the specified uninstall mouse hook.
        /// </summary>
        /// <param name="UninstallMouseHook">if set to <c>true</c> [uninstall mouse hook].</param>
        /// <param name="UninstallKeyboardHook">if set to <c>true</c> [uninstall keyboard hook].</param>
        /// <param name="ThrowExceptions">if set to <c>true</c> [throw exceptions].</param>
        /// <exception cref="Win32Exception">
        /// </exception>
        public void Stop(bool UninstallMouseHook, bool UninstallKeyboardHook, bool ThrowExceptions)
        {
            if (hMouseHook != 0 && UninstallMouseHook)
            {
                var retMouse = NativeMethods.UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
                if (retMouse == 0 && ThrowExceptions)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }

            if (hKeyboardHook != 0 && UninstallKeyboardHook)
            {
                var retKeyboard = NativeMethods.UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
                if (retKeyboard == 0 && ThrowExceptions)
                {
                    var errorCode = Marshal.GetLastWin32Error();
                    throw new Win32Exception(errorCode);
                }
            }
        }
        //-------------------------------------------------------------------------------  

        /// <summary>
        ///     Mouses the hook proc.
        /// </summary>
        /// <param name="nCode">The n code.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>System.Int32.</returns>
        private int MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0 && OnMouseActivity != null)
            {
                var mouseHookStruct = (MouseLLHookStruct) Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));

                var button = MouseButtons.None;
                short mouseDelta = 0;
                var clickCount = 0;
                switch (wParam)
                {
                    case (int) WindowMessage.WM_LBUTTONDOWN: //513出现了  
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case (int) WindowMessage.WM_RBUTTONDOWN: //516出现了  
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case (int) WindowMessage.WM_LBUTTONDBLCLK: //515  doubleclick没有出现过  
                        button = MouseButtons.XButton1;
                        clickCount = 2;
                        break;
                    case (int) WindowMessage.WM_RBUTTONDBLCLK: //518  
                        button = MouseButtons.XButton1;
                        clickCount = 2;
                        break;
                    case (int) WindowMessage.WM_MOUSEMOVE: //512 出现了  
                        button = MouseButtons.XButton2;
                        clickCount = 0;
                        break;
                    case (int) WindowMessage.WM_MOUSEWHEEL: //522 没试  
                        mouseDelta = (short) ((mouseHookStruct.mouseData >> 16) & 0xffff);
                        clickCount = 0;
                        break;
                }

                var e = new MouseEventArgs(
                    button,
                    clickCount,
                    mouseHookStruct.pt.x,
                    mouseHookStruct.pt.y,
                    mouseDelta);
                OnMouseActivity(this, e); //转给委托函数  
            }
            return NativeMethods.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }

        //------------------------------------------------------------------------------------  
        /// <summary>
        ///     Keyboards the hook proc.
        /// </summary>
        /// <param name="nCode">The n code.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns>System.Int32.</returns>
        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            var handled = false;
            if (nCode >= 0 && (KeyDown != null || KeyUp != null || KeyPress != null))
            {
                var MyKeyboardHookStruct =
                    (KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                if (KeyDown != null && (wParam == (int) WindowMessage.WM_KEYDOWN ||
                                        wParam == (int) WindowMessage.WM_SYSKEYDOWN))
                {
                    var keyData = (Keys) MyKeyboardHookStruct.vkCode;
                    var e = new KeyEventArgs(keyData);
                    KeyDown(this, e); //转给委托函数  
                    handled = handled || e.Handled;
                }

                if (KeyPress != null && wParam == (int) WindowMessage.WM_KEYDOWN)
                {
                    var isDownShift = (NativeMethods.GetKeyState(NativeMethods.VK_SHIFT) & 0x80) == 0x80 ? true : false;
                    var isDownCapslock = NativeMethods.GetKeyState(NativeMethods.VK_CAPITAL) != 0 ? true : false;

                    var keyState = new byte[256];
                    NativeMethods.GetKeyboardState(keyState);
                    var inBuffer = new byte[2];
                    if (NativeMethods.ToAscii(MyKeyboardHookStruct.vkCode,
                            MyKeyboardHookStruct.scanCode,
                            keyState,
                            inBuffer,
                            MyKeyboardHookStruct.flags) == 1)
                    {
                        var key = (char) inBuffer[0];
                        if (isDownCapslock ^ isDownShift && char.IsLetter(key)) key = char.ToUpper(key);
                        var e = new KeyPressEventArgs(key);
                        KeyPress(this, e);
                        handled = handled || e.Handled;
                    }
                }

                if (KeyUp != null &&
                    (wParam == (int) WindowMessage.WM_KEYUP || wParam == (int) WindowMessage.WM_SYSKEYUP))
                {
                    var keyData = (Keys) MyKeyboardHookStruct.vkCode;
                    var e = new KeyEventArgs(keyData);
                    KeyUp(this, e);
                    handled = handled || e.Handled;
                }
            }

            if (handled)
                return 1;
            return NativeMethods.CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }

        #endregion
    }
}