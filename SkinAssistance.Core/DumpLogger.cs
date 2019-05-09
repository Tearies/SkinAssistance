using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SkinAssistance.Core
{
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