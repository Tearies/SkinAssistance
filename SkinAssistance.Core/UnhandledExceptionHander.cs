using System;
using System.Diagnostics;
using System.IO;
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
        public enum ExceptionMode : uint
        {
            /// <summary>
            /// 表示我已经处理了异常,可以优雅地结束了  
            /// </summary>
            EXCEPTION_EXECUTE_HANDLER = 1,
            /// <summary>
            /// 表示我不处理,其他人来吧,于是windows调用默认的处理程序显示一个错误框,并结束  
            /// </summary>
            EXCEPTION_CONTINUE_SEARCH = 0,
            /// <summary>
            /// 表示错误已经被修复,请从异常发生处继续执行
            /// </summary>
            EXCEPTION_CONTINUE_EXECUTION = 0xffffffff
        }

        /// <summary>
        /// <para>默认情况下，子进程继承父进程的错误模式标志。</para>
        /// <para>换句话说，如果一个进程SEM_NOGPFAULTERRORBOX标志已经打开，并且生成了一个子进程，该子进程也拥有这个打开的标志。</para>
        /// <para>但是，子进程并没有得到这一情况的通知，它可能尚未编写以便处理GP故障的错误。</para>
        /// <para>如果GP故障发生在子进程的某个线程中，该子进程就会终止运行，而不通知用户。</para>
        /// <para>父进程可以防止子进程继承它的错误模式，方法是在调用CreateProcess时设定CREATE_DEFAULT_ERROR_MODE标志</para>
        /// </summary>
        [Flags]
        public enum ErrorMode : int
        {
            /// <summary>
            /// 0x0 <para>使用系统默认的，既显示所有错误的对话框</para>
            /// </summary>
            SEM_Default = 0,
            /// <summary>
            /// 0x0001 <para>系统不显示关键错误处理消息框。 相反，系统发送错误给调用进程</para>
            /// </summary>
            SEM_FAILCRITICALERRORS = 0x0001,

            /// <summary>
            /// 0x0004 <para>系统会自动修复故障此功能只支持部分处理器架构</para>
            /// </summary>
            SEM_NOALIGNMENTFAULTEXCEPT = 0x0004,

            /// <summary>
            /// 0x0002 <para>系统不显示Windows错误报告对话框</para>
            /// </summary>
            SEM_NOGPFAULTERRORBOX = 0x0002,

            /// <summary>
            /// 0x8000 <para>当无法找到文件时不弹出错误对话框。 相反，错误返回给调用进程</para>
            /// </summary>
            SEM_NOOPENFILEERRORBOX = 0x8000
        }

        /// <summary>
        /// 错误处理模式
        /// </summary>
        [Flags]
        public enum EnumErrorHandle : uint
        {
            None = 0x01,
            Exit = 0x10,
            Restart = 0x100,

            Default = Exit | Restart,
        }
        /// <summary>
        /// 异常来源
        /// </summary>
        public enum ExceptionSources : uint
        {
            /// <summary>
            /// The UI dispatcher
            /// </summary>
            Dispatcher,
            /// <summary>
            /// The task
            /// </summary>
            Task,
            /// <summary>
            /// The application domain
            /// </summary>
            AppDomain,
            /// <summary>
            /// The seh
            /// </summary>
            SEH
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EXCEPTION_POINTERS
        {
            public EXCEPTION_RECORD ExceptionRecord;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct EXCEPTION_RECORD
        {
            /// <summary>
            /// 异常代码的指针地址
            /// </summary>
            public IntPtr ExceptionCode;

        }
    }
    /// <summary>
    /// 异常的回调Handler
    /// </summary>
    /// <param name="innerException">The inner exception.</param>
    /// <param name="handle">The handle.</param>
    /// <param name="exSource">The ex source.</param>
    public delegate void ExceptionEventHandler(Exception innerException, EnumErrorHandle handle, ExceptionSources exSource);

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

    public class DumpLogger
    {
        private static readonly string DumpPath;

        static DumpLogger()
        {
            DumpPath = Environment.CurrentDirectory + "\\Dump\\";
            if (!Directory.Exists(DumpPath))
            {
                Directory.CreateDirectory(DumpPath);
            }
        }
        [Flags]
        private enum MiniDumpTypes : uint
        {
            // From dbghelp.h:
            Normal = 0x00000000,
            DataSegs = 0x00000001,
            FullMemory = 0x00000002,
            HandleData = 0x00000004,
            FilterMemory = 0x00000008,
            ScanMemory = 0x00000010,
            UnloadedModules = 0x00000020,
            IndirectlyReferencedMemory = 0x00000040,
            ErModulePaths = 0x00000080,
            ProcessThreadData = 0x00000100,
            PrivateReadWriteMemory = 0x00000200,
            OutOptionalData = 0x00000400,
            FullMemoryInfo = 0x00000800,
            ThreadInfo = 0x00001000,
            CodeSegs = 0x00002000,
            OutAuxiliaryState = 0x00004000,
            FullAuxiliaryState = 0x00008000,
            PrivateWriteCopyMemory = 0x00010000,
            IgnoreInaccessibleMemory = 0x00020000,
            ValidTypeFlags = 0x0003ffff,
        };


        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        struct MiniDumpExceptionInformation
        {
            public uint ThreadId;
            public IntPtr ExceptioonPointers;
            [MarshalAs(UnmanagedType.Bool)]
            public bool ClientPointers;
        }


        [DllImport("dbghelp.dll",
            EntryPoint = "MiniDumpWriteDump",
            CallingConvention = CallingConvention.StdCall,
            CharSet = CharSet.Unicode,
            ExactSpelling = true, SetLastError = true)]
        private static extern bool MiniDumpWriteDump(
            IntPtr hProcess,
            uint processId,
            IntPtr hFile,
            uint dumpType,
            ref MiniDumpExceptionInformation expParam,
            IntPtr userStreamParam,
            IntPtr callbackParam);

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        private static extern uint GetCurrentThreadId();

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentProcess", ExactSpelling = true)]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", EntryPoint = "GetCurrentProcessId", ExactSpelling = true)]
        private static extern uint GetCurrentProcessId();

        public static bool Write(string filename, IntPtr exceptionPointer)
        {
            string fileName = DumpPath + GetCurrentProcessId() + "." + filename + ".dmp";
            using (var fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
            {
                MiniDumpExceptionInformation exp;
                exp.ThreadId = GetCurrentThreadId();
                exp.ClientPointers = false;
                exp.ExceptioonPointers = exceptionPointer;
                bool bRet = MiniDumpWriteDump(
                    GetCurrentProcess(),
                    GetCurrentProcessId(),
                    fs.SafeFileHandle.DangerousGetHandle(),
                    (uint)MiniDumpTypes.FullMemory,
                    ref exp,
                    IntPtr.Zero,
                    IntPtr.Zero);
                return bRet;
            }
        }
    }
}
