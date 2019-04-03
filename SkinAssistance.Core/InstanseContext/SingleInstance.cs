#region NS

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Microsoft.VisualBasic.ApplicationServices;
using SkinAssistance.Core.Extensions;
using ShutdownMode = Microsoft.VisualBasic.ApplicationServices.ShutdownMode;
using StartupEventArgs = Microsoft.VisualBasic.ApplicationServices.StartupEventArgs;
using UnhandledExceptionEventArgs = Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs;

#endregion

namespace SkinAssistance.Core.InstanseContext
{
    #region Reference

    #endregion

    internal class SingleInstance<TApplication> : ApplicationService.WindowsFormsApplicationBase
        where TApplication : Application
    {
        #region Properties

        private Action FirstInstanceRunAction { get; set; }
        private Action<StartupNextInstanceEventArgs> NextInstanceRunAction { get; set; }

        #endregion

        #region Method

        public SingleInstance(Action firstInstanceRunAction, Action<StartupNextInstanceEventArgs> nextInstanceRunAction)
            : base(AuthenticationMode.Windows)
        {
            UnhandledException += SingleInstance_UnhandledException;
            IsSingleInstance = true;
            ShutdownStyle = ShutdownMode.AfterMainFormCloses;
            FirstInstanceRunAction = firstInstanceRunAction;
            NextInstanceRunAction = nextInstanceRunAction;
        }

        private void SingleInstance_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            e.ExitApplication = false;
        }

        protected override bool OnInitialize(ReadOnlyCollection<string> commandLineArgs)
        {
            Trace.WriteLine("Inite Application");
            return base.OnInitialize(commandLineArgs);
        }

        protected override void OnRun()
        {
            //  Trace.WriteLine("Run Application");
            base.OnRun();
        }

        protected override bool OnStartup(StartupEventArgs eventArgs)
        {
            FirstInstanceRunAction();
            return false;
        }

        protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            // eventArgs.BringToForeground = true;
            NextInstanceRunAction(eventArgs);

            if (eventArgs.BringToForeground)
            {
                if (Application.Current.MainWindow != null)
                {
                    if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
                        Application.Current.MainWindow.WindowState = WindowState.Normal;
                    Application.Current.MainWindow.ActiveWindow();
                }
                eventArgs.BringToForeground = false;
            }
            base.OnStartupNextInstance(eventArgs);
        }


        /// <summary>
        ///     重写产生异常的方法
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        protected override bool OnUnhandledException(UnhandledExceptionEventArgs e)
        {
            e.ExitApplication = false;
            //  LogWriter.CriticalLogInfo("Application Start failed, Exception->{0}", e.Exception);
            //方法会终止进程
            // Environment.FailFast("Application Start faild", e.Exception);
            // Environment.Exit(-1);
            // 返回值 不起作用
            return e.ExitApplication;
        }

        #endregion
    }
}