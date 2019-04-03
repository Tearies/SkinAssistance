#region NS

using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;
using SkinAssistance.Core.Native;

#endregion

namespace SkinAssistance.Core.Extensions
{
    /// <summary>
    /// </summary>
    public static class WindowExtension
    {
        #region Method

        /// <summary>
        /// </summary>
        /// <param name="window"></param>
        public static void ActiveWindow(this Window window)
        {
            var handle = new WindowInteropHelper(window).Handle;
            if (window.WindowState == WindowState.Minimized)
            {
                NativeMethods.ShowWindow(handle, SW.RESTORE);
            }
            else
            {
                var currentForgourndHandle = NativeMethods.GetForegroundWindow();
                var forgourndprocessId = 0;
                var cureentForgourndThreadId =
                    NativeMethods.GetWindowThreadProcessId(currentForgourndHandle, out forgourndprocessId);
                var currentThreadId = NativeMethods.GetCurrentThreadId();
                NativeMethods.AttachThreadInput((int)currentThreadId, cureentForgourndThreadId, true);
                NativeMethods.SetForegroundWindow(handle);
                NativeMethods.SetFocus(handle);
                NativeMethods.AttachThreadInput((int)currentThreadId, cureentForgourndThreadId, false);
            }
        }

        /// <summary>
        /// Processes all UI messages currently in the message queue.
        /// </summary>

        public static void DoEvents(this Application application)
        {
            Action action = delegate { };
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Input, action);
        }

        #endregion
    }
}