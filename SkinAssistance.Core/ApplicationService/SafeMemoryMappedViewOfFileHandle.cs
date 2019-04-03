#region NS

using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;
using SkinAssistance.Core.Native;

#endregion

namespace SkinAssistance.Core.ApplicationService
{
    #region Reference

    #endregion

    [SecurityCritical]
    internal sealed class SafeMemoryMappedViewOfFileHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        #region Method

        internal SafeMemoryMappedViewOfFileHandle() : base(true)
        {
        }

        internal SafeMemoryMappedViewOfFileHandle(IntPtr handle, bool ownsHandle) : base(ownsHandle)
        {
            SetHandle(handle);
        }

        [SecurityCritical]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle()
        {
            bool flag;
            try
            {
                if (UnsafeNativeMethods.UnmapViewOfFile(handle))
                    return true;
                flag = false;
            }
            finally
            {
                handle = IntPtr.Zero;
            }
            return flag;
        }

        #endregion
    }
}