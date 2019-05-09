using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SkinAssistance.Core.Native;
using SkinAssistance.Core.VBI.Presentation.Core;

namespace SkinAssistance.Core
{
    using System;
    using System.Runtime.InteropServices;

    namespace VBI.Presentation.Core
    {
    }

    /// <summary>
    /// 全局异常处理
    /// </summary>
    public static class UnhandledExceptionHander
    {
        private static ExceptionEventHandler _onExceptionEventHandler;
        private static EnumErrorHandle _ErrorHandle;
        private static int _MaxErrorWindowCount = 10;
        private static int _ErrorWindowCount = 0;

        /// <summary>
        /// 一个默认的异常回调处理
        /// </summary>
        public static ExceptionEventHandler DefaultExcetionHandle { get; private set; }

        static UnhandledExceptionHander()
        {
            DefaultExcetionHandle = new ExceptionEventHandler((ex, handle, exSource) =>
            {
                switch (handle)
                {
                    case EnumErrorHandle.Exit:
                        Application.Current.Shutdown(); ;
                        break;
                    case EnumErrorHandle.Restart:
                        Restart();
                        break;
                }
            });
        }
        public static void Restart()
        {
            if (Assembly.GetEntryAssembly() == null)
                throw new NotSupportedException("RestartNotSupported");
            var flag = false;

            var commandLineArgs = Environment.GetCommandLineArgs();
            var stringBuilder1 = new StringBuilder((commandLineArgs.Length - 1) * 16);
            for (var index = 1; index < commandLineArgs.Length - 1; ++index)
            {
                stringBuilder1.Append('"');
                stringBuilder1.Append(commandLineArgs[index]);
                stringBuilder1.Append("\" ");
            }
            if (commandLineArgs.Length > 1)
            {
                stringBuilder1.Append('"');
                var stringBuilder2 = stringBuilder1;
                var strArray = commandLineArgs;
                var index = strArray.Length - 1;
                var str = strArray[index];
                stringBuilder2.Append(str);
                stringBuilder1.Append('"');
            }
            var startInfo = Process.GetCurrentProcess().StartInfo;
            startInfo.FileName = Assembly.GetEntryAssembly().Location;
            if (stringBuilder1.Length > 0)
                startInfo.Arguments = stringBuilder1.ToString();
            Application.Current.Shutdown();
            Process.Start(startInfo);
        }
        /// <summary>
        /// 注册全局异常处理。
        /// onUnhandledException 异常处理事件，如果未null则使用DefaultExcetionHandle
        /// </summary>
        public static void RegisterGlobalException(ExceptionEventHandler onUnhandledException = null, EnumErrorHandle handle = EnumErrorHandle.Default)
        {
            _onExceptionEventHandler = onUnhandledException ?? DefaultExcetionHandle;
            _ErrorHandle = handle;
            //AppDomain中的异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //主UI线程
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            //task scheduler 
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            //SEH
            RegisterSEHException();
        }
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var ex = e.ExceptionObject as Exception;
            if (ex == null) ex = new Exception("ExceptionObject is Null");
            if (!e.IsTerminating)
            {
                HandleException(ex, ExceptionSources.AppDomain);
            }
            else
            {
                HandleFatal(ExceptionSources.AppDomain, Marshal.GetExceptionPointers(), ex);
            }
        }
        private static void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            HandleException(e.Exception, ExceptionSources.Dispatcher);

        }
        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            HandleException(e.Exception, ExceptionSources.Task);
            e.SetObserved();
        }

        /// <summary>
        /// 让代理程序处理异常
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="exSource"></param>
        [HandleProcessCorruptedStateExceptions]
        private static void OnHandedExceptionProxy(Exception exception, ExceptionSources exSource, EnumErrorHandle handle)
        {
            try
            {
                var handler = _onExceptionEventHandler;
                if (handler != null)
                    handler(exception, handle, exSource);
            }
            catch (Exception e)
            {
                HandleException(e, exSource, false);
            }
        }

        #region SEH Exception

        [DllImport("kernel32")]
        private static extern UnhandledExceptionCallback SetUnhandledExceptionFilter(UnhandledExceptionCallback callback);
        [DllImport("kernel32", SetLastError = true)]
        static extern ErrorMode SetErrorMode(ErrorMode wMode);

        private delegate ExceptionMode UnhandledExceptionCallback(IntPtr ExceptionPointers);
        private static UnhandledExceptionCallback _callback;

        private static void RegisterSEHException()
        {
            _callback = SEHExceptionHandler;
            SetUnhandledExceptionFilter(_callback);
        }
        [HandleProcessCorruptedStateExceptions]
        private static ExceptionMode SEHExceptionHandler(IntPtr exceptionPointer)
        {
            try
            {
                HandleFatal(ExceptionSources.SEH, exceptionPointer);
            }
            catch (Exception ex)
            {
                HandleFatal(ExceptionSources.SEH, IntPtr.Zero, ex);
            }

            //SetErrorMode(SetErrorMode(ErrorMode.SEM_Default) | ErrorMode.SEM_NOGPFAULTERRORBOX);
            return ExceptionMode.EXCEPTION_EXECUTE_HANDLER;
        }

        #endregion

        /*********************************  异常处理  **********************************/
        /// <summary>
        /// 处理异常
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exSource"></param>
        /// <param name="proxy"></param>
        private static void HandleException(Exception ex, ExceptionSources exSource, bool proxy = true)
        {
            try
            {
              
            }
            catch (Exception ex1)
            { }

        }
        /// <summary>
        /// 处理致命错误
        /// </summary>
        private static void HandleFatal(ExceptionSources source, IntPtr exceptionPointer, Exception hex = null)
        {
            try
            {
                int lastErrorCode = 0;
                if (exceptionPointer != IntPtr.Zero)
                {
                    var exInfo = Marshal.PtrToStructure<EXCEPTION_POINTERS>(exceptionPointer);
                    lastErrorCode = Marshal.ReadInt32(exInfo.ExceptionRecord.ExceptionCode);
                }
                StringBuilder error = new StringBuilder();
                error.AppendLine("系统意外崩溃，您可以尝试重启程序，或者联系技术支持");
                error.AppendLine("参考错误:0x" + (lastErrorCode == 0 ? Marshal.GetExceptionCode() : lastErrorCode).ToString("X8"));
                error.AppendLine("错误线程:0x" + ThreadHelper.GetCurrentThreadID());
                error.AppendLine("错误来源:" + source);
                error.AppendLine("内存占用:" + Environment.WorkingSet / 1024 + "KB");
                if (hex != null)
                {
                    error.AppendLine("附加信息:\n" + hex.StackTrace);
                }
                else
                {
                    error.AppendLine("附加信息:\n" + new StackTrace());
                }

                Log.Error(error.ToString());
                try
                {
                    
                        Log.Error("Log dump start...");
                        DumpLogger.Write(string.Format("{0}.{1}", DateTime.Now.ToFileTime(), Guid.NewGuid().ToString()), exceptionPointer);
                        Log.Error("Log dump succ");
                 
                }
                catch (Exception ex)
                {
                    Log.Error("log dump failed", ex);
                }
              
            }
            catch (Exception ex) { }
        }
    }
}
