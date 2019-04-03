// ***********************************************************************
// Assembly         : VBI.Player.Core
// Author           : Tearies
// Created          : 11-10-2017
//
// Last Modified By : Tearies
// Last Modified On : 11-10-2017
// ***********************************************************************
// <copyright file="WindowMessage.cs" company="北京恒泰实达科技股份有限公司">
//     Copyright (c) 北京恒泰实达科技股份有限公司. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace SkinAssistance.Core.Native
{
    /// <summary>
    ///     Enum WindowMessage
    /// </summary>
    internal enum WindowMessage
    {
        /// <summary>
        ///     The wm activateapp
        /// </summary>
        WM_ACTIVATEAPP = 0x001C,

        /// <summary>
        ///     The wm mouseactivate
        /// </summary>
        WM_MOUSEACTIVATE = 0x0021,

        /// <summary>
        ///     The wm windowposchanging
        /// </summary>
        WM_WINDOWPOSCHANGING = 0x0046,

        /// <summary>
        ///     The wm syskeydown
        /// </summary>
        WM_SYSKEYDOWN = 0x0104,

        /// <summary>
        ///     The wm syskeyup
        /// </summary>
        WM_SYSKEYUP = 0x0105,

        /// <summary>
        ///     The wm mousemove
        /// </summary>
        WM_MOUSEMOVE = 0x0200,

        /// <summary>
        ///     The wm lbuttondown
        /// </summary>
        WM_LBUTTONDOWN = 0x0201,

        /// <summary>
        ///     The wm lbuttondblclk
        /// </summary>
        WM_LBUTTONDBLCLK = 0x0203,

        /// <summary>
        ///     The wm rbuttondown
        /// </summary>
        WM_RBUTTONDOWN = 0x0204,

        /// <summary>
        ///     The wm rbuttondblclk
        /// </summary>
        WM_RBUTTONDBLCLK = 0x0206,

        /// <summary>
        ///     The wm mousewheel
        /// </summary>
        WM_MOUSEWHEEL = 0x020A,

        /// <summary>
        ///     The wm print
        /// </summary>
        WM_PRINT = 0x0317,

        /// <summary>
        ///     The wm keydown
        /// </summary>
        WM_KEYDOWN = 0x100,

        /// <summary>
        ///     The wm keyup
        /// </summary>
        WM_KEYUP = 0x101
    }
}