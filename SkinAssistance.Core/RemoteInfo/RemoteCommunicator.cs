#region NS

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using SkinAssistance.Core.ApplicationService;

#endregion

namespace SkinAssistance.Core.RemoteInfo
{
    #region Reference

    #endregion

    internal class RemoteCommunicator : MarshalByRefObject
    {
        #region Properties

        private readonly AsyncOperation m_AsyncOp;
        private readonly EventWaitHandle m_ConnectionMadeSemaphore;
        private readonly WindowsIdentity m_OriginalUser;
        private readonly SendOrPostCallback m_StartNextInstanceDelegate;

        #endregion

        #region Method

        [SecurityCritical]
        internal RemoteCommunicator(WindowsFormsApplicationBase appObject, EventWaitHandle ConnectionMadeSemaphore)
        {
            new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Assert();
            m_OriginalUser = WindowsIdentity.GetCurrent();
            CodeAccessPermission.RevertAssert();
            m_AsyncOp = AsyncOperationManager.CreateOperation(null);
            m_StartNextInstanceDelegate = appObject.RunNextInstanceDelegate;
            m_ConnectionMadeSemaphore = ConnectionMadeSemaphore;
        }

        [SecuritySafeCritical]
        [OneWay]
        public void RunNextInstance(ReadOnlyCollection<string> Args)
        {
            new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Assert();
            if (m_OriginalUser.User != WindowsIdentity.GetCurrent().User)
                return;
            m_ConnectionMadeSemaphore.Set();
            CodeAccessPermission.RevertAssert();
            m_AsyncOp.Post(m_StartNextInstanceDelegate, Args);
        }

        [SecurityCritical]
        public override object InitializeLifetimeService()
        {
            return null;
        }

        #endregion
    }
}