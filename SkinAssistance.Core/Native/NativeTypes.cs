#region NS

using System;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

#endregion

namespace SkinAssistance.Core.Native
{
    #region Reference

    #endregion

    [EditorBrowsable(EditorBrowsableState.Never)]
    internal sealed class NativeTypes
    {
        #region Properties

        internal const int ERROR_ACCESS_DENIED = 5;
        internal const int ERROR_ALREADY_EXISTS = 0xb7;
        internal const int ERROR_CANCELLED = 0x4c7;
        internal const int ERROR_FILE_EXISTS = 80;
        internal const int ERROR_FILE_NOT_FOUND = 2;
        internal const int ERROR_FILENAME_EXCED_RANGE = 0xce;
        internal const int ERROR_INVALID_DRIVE = 15;
        internal const int ERROR_INVALID_PARAMETER = 0x57;
        internal const int ERROR_OPERATION_ABORTED = 0x3e3;
        internal const int ERROR_PATH_NOT_FOUND = 3;
        internal const int ERROR_SHARING_VIOLATION = 0x20;
        internal const int GW_CHILD = 5;
        internal const int GW_HWNDFIRST = 0;
        internal const int GW_HWNDLAST = 1;
        internal const int GW_HWNDNEXT = 2;
        internal const int GW_HWNDPREV = 3;
        internal const int GW_MAX = 5;
        internal const int GW_OWNER = 4;
        internal const int LCMAP_FULLWIDTH = 0x800000;
        internal const int LCMAP_HALFWIDTH = 0x400000;
        internal const int LCMAP_HIRAGANA = 0x100000;
        internal const int LCMAP_KATAKANA = 0x200000;
        internal const int LCMAP_LOWERCASE = 0x100;
        internal const int LCMAP_SIMPLIFIED_CHINESE = 0x2000000;
        internal const int LCMAP_TRADITIONAL_CHINESE = 0x4000000;
        internal const int LCMAP_UPPERCASE = 0x200;
        internal const int NORMAL_PRIORITY_CLASS = 0x20;
        internal const int STARTF_USESHOWWINDOW = 1;
        internal static readonly IntPtr INVALID_HANDLE = new IntPtr(-1);

        #endregion

        #region Nested Types

        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        internal sealed class LateInitSafeHandleZeroOrMinusOneIsInvalid : SafeHandleZeroOrMinusOneIsInvalid
        {
            #region Method

            [SecurityCritical]
            internal LateInitSafeHandleZeroOrMinusOneIsInvalid() : base(true)
            {
            }

            [SecurityCritical]
            internal void InitialSetHandle(IntPtr h)
            {
                SetHandle(h);
            }

            [SecurityCritical]
            protected override bool ReleaseHandle()
            {
                return (NativeMethods.CloseHandle(handle) > 0);
            }

            #endregion
        }

        [Flags]
        internal enum MoveFileExFlags
        {
            MOVEFILE_COPY_ALLOWED = 2,
            MOVEFILE_DELAY_UNTIL_REBOOT = 4,
            MOVEFILE_REPLACE_EXISTING = 1,
            MOVEFILE_WRITE_THROUGH = 8
        }

        [StructLayout(LayoutKind.Sequential)]
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        internal sealed class PROCESS_INFORMATION
        {
            #region Properties

            public int dwProcessId;
            public int dwThreadId;
            public IntPtr hProcess = IntPtr.Zero;
            public IntPtr hThread = IntPtr.Zero;

            #endregion
        }

        [StructLayout(LayoutKind.Sequential)]
        internal sealed class SECURITY_ATTRIBUTES : IDisposable
        {
            #region Properties

            public bool bInheritHandle;
            public IntPtr lpSecurityDescriptor;
            public int nLength = Marshal.SizeOf(typeof(SECURITY_ATTRIBUTES));

            #endregion

            #region Override

            [SecuritySafeCritical]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            public void Dispose()
            {
                if (lpSecurityDescriptor != IntPtr.Zero)
                {
                    UnsafeNativeMethods.LocalFree(lpSecurityDescriptor);
                    lpSecurityDescriptor = IntPtr.Zero;
                }
                GC.SuppressFinalize(this);
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        [SecurityCritical]
        [SuppressUnmanagedCodeSecurity]
        internal sealed class STARTUPINFO : IDisposable
        {
            #region Properties

            public int cb;
            public short cbReserved2;
            public int dwFillAttribute;
            public int dwFlags;
            public int dwX;
            public int dwXCountChars;
            public int dwXSize;
            public int dwY;
            public int dwYCountChars;
            public int dwYSize;
            public IntPtr hStdError = IntPtr.Zero;
            public IntPtr hStdInput = IntPtr.Zero;
            public IntPtr hStdOutput = IntPtr.Zero;
            public IntPtr lpDesktop = IntPtr.Zero;
            public IntPtr lpReserved = IntPtr.Zero;
            public IntPtr lpReserved2 = IntPtr.Zero;
            public IntPtr lpTitle = IntPtr.Zero;
            private bool m_HasBeenDisposed;
            public short wShowWindow;

            #endregion

            #region Override

            [SecuritySafeCritical]
            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            #endregion

            #region Method

            [SecurityCritical]
            private void Dispose(bool disposing)
            {
                if (!m_HasBeenDisposed && disposing)
                {
                    m_HasBeenDisposed = true;
                    if ((dwFlags & 0x100) != 0)
                    {
                        if ((hStdInput != IntPtr.Zero) && (hStdInput != INVALID_HANDLE))
                        {
                            NativeMethods.CloseHandle(hStdInput);
                            hStdInput = INVALID_HANDLE;
                        }
                        if ((hStdOutput != IntPtr.Zero) && (hStdOutput != INVALID_HANDLE))
                        {
                            NativeMethods.CloseHandle(hStdOutput);
                            hStdOutput = INVALID_HANDLE;
                        }
                        if ((hStdError != IntPtr.Zero) && (hStdError != INVALID_HANDLE))
                        {
                            NativeMethods.CloseHandle(hStdError);
                            hStdError = INVALID_HANDLE;
                        }
                    }
                }
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential)]
        internal sealed class SystemTime
        {
            #region Properties

            public short wDay;
            public short wDayOfWeek;
            public short wHour;
            public short wMilliseconds;
            public short wMinute;
            public short wMonth;
            public short wSecond;
            public short wYear;

            #endregion
        }

        #endregion

        #region Method

        private NativeTypes()
        {
        }

        #endregion
    }

    
}