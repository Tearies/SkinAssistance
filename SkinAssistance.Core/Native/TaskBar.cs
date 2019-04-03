using System;

namespace SkinAssistance.Core.Native
{
    public class TaskBar
    {

        private const string TaskBarClassName = "Shell_TrayWnd";



        private TaskBar()
        {
            
        }
        //显示任务栏 
        public static void Show()
        {
            IntPtr hTray = NativeMethods.FindWindowA(TaskBarClassName, string.Empty);
            NativeMethods.ShowWindow(hTray, SW.SHOW);
        }

        //隐藏任务栏 
        public static void Hide()
        {
            IntPtr hTray = NativeMethods.FindWindowA(TaskBarClassName, string.Empty);
            NativeMethods.ShowWindow(hTray, SW.HIDE);
        }

        //设置自动隐藏 
        public static bool AutoHide
        {
            set
            {
                Appbardata abd = new Appbardata();
                abd.hwnd = NativeMethods.FindWindowA(TaskBarClassName, string.Empty);
                abd.lParam = value ? NativeMethods.ABS_ONTOP : NativeMethods.ABS_NOTONTOP;
                NativeMethods.SHAppBarMessage(NativeMethods.ABM_SETSTATE, ref abd);
            }
        }

        //设置任务栏置顶 
        public static bool TopMost
        {
            set
            {
                Appbardata abd = new Appbardata();
                abd.hwnd = NativeMethods.FindWindowA(TaskBarClassName, string.Empty);
                abd.lParam = value? NativeMethods.ABS_ONTOP : NativeMethods.ABS_NOTONTOP;
                NativeMethods.SHAppBarMessage(NativeMethods.ABM_SETSTATE, ref abd);
            }
        }

     
        
    }
}