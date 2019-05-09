#region NS

using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;

#endregion

namespace SkinAssistance.Core.RemoteInfo
{
    #region Reference

    #endregion

    [HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
    public class NetworkInfo
    {
        #region Properties

        private readonly object m_SyncObject;
        private bool m_Connected;
        private SendOrPostCallback m_NetworkAvailabilityChangedCallback;
        private ArrayList m_NetworkAvailabilityEventHandlers;
        private byte[] m_PingBuffer;
        private SynchronizationContext m_SynchronizationContext;

        /// <summary>
        ///     Indicates whether a computer is connected to a network.
        /// </summary>
        /// <returns>
        ///     True if the computer is connected to a network; otherwise False.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        /// <PermissionSet>
        ///     <IPermission
        ///         class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        ///     <IPermission
        ///         class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" PathDiscovery="*AllFiles*" />
        ///     <IPermission
        ///         class="System.Security.Permissions.RegistryPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Unrestricted="true" />
        ///     <IPermission
        ///         class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Flags="UnmanagedCode" />
        ///     <IPermission
        ///         class="System.Net.NetworkInformation.NetworkInformationPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"
        ///         version="1" Access="Read" />
        /// </PermissionSet>
        public static bool IsAvailable
        {
            get
            {
                return NetWorkInfomation.IsConnectd;
            }
        }
 
        private byte[] PingBuffer
        {
            get
            {
                if (m_PingBuffer == null)
                {
                    m_PingBuffer = new byte[32];
                    var index = 0;
                    do
                    {
                        m_PingBuffer[index] = Convert.ToByte(checked(97 + index % 23),
                            CultureInfo.InvariantCulture);
                        checked
                        {
                            ++index;
                        }
                    } while (index <= 31);
                }
                return m_PingBuffer;
            }
        }
        private const int BUFFER_SIZE = 32;
        private const int DEFAULT_TIMEOUT = 100000;
        private const int DEFAULT_PING_TIMEOUT = 1000;
        private const string DEFAULT_USERNAME = "";
        private const string DEFAULT_PASSWORD = "";

        #endregion

        #region Method

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:Microsoft.VisualBasic.Devices.Network" /> class.
        /// </summary>
        public NetworkInfo()
        {
            m_SyncObject = new object();
        }

        /// <summary>
        ///     Occurs when the network availability changes.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        public event NetworkAvailableEventHandler NetworkAvailabilityChanged
        {
            add
            {
                try
                {
                    m_Connected = IsAvailable;
                }
                catch (SecurityException ex)
                {
                    return;
                }
                catch (PlatformNotSupportedException ex)
                {
                    return;
                }
                var Expression = m_SyncObject;
                ObjectFlowControl.CheckForSyncLockOnValueType(Expression);
                var lockTaken = false;
                try
                {
                    Monitor.Enter(Expression, ref lockTaken);
                    if (m_NetworkAvailabilityEventHandlers == null)
                        m_NetworkAvailabilityEventHandlers = new ArrayList();
                    m_NetworkAvailabilityEventHandlers.Add(value);
                    if (m_NetworkAvailabilityEventHandlers.Count != 1)
                        return;
                    m_NetworkAvailabilityChangedCallback = NetworkAvailabilityChangedHandler;
                    if (AsyncOperationManager.SynchronizationContext == null)
                        return;
                    m_SynchronizationContext = AsyncOperationManager.SynchronizationContext;
                    try
                    {
                        NetworkChange.NetworkAddressChanged += OS_NetworkAvailabilityChangedListener;
                    }
                    catch (PlatformNotSupportedException ex)
                    {
                    }
                    catch (NetworkInformationException ex)
                    {
                    }
                }
                finally
                {
                    if (lockTaken)
                        Monitor.Exit(Expression);
                }
            }
            remove
            {
                if (m_NetworkAvailabilityEventHandlers == null || m_NetworkAvailabilityEventHandlers.Count <= 0)
                    return;
                m_NetworkAvailabilityEventHandlers.Remove(value);
                if (m_NetworkAvailabilityEventHandlers.Count != 0)
                    return;
                NetworkChange.NetworkAddressChanged -= OS_NetworkAvailabilityChangedListener;
                DisconnectListener();
            }
        }

        [SpecialName]
        private void raise_NetworkAvailabilityChanged(object sender, NetworkAvailableEventArgs e)
        {
            if (m_NetworkAvailabilityEventHandlers == null)
                return;
            try
            {
                foreach (NetworkAvailableEventHandler availableEventHandler in m_NetworkAvailabilityEventHandlers)
                    if (availableEventHandler != null)
                        availableEventHandler(sender, e);
            }
            finally
            {
                IEnumerator enumerator = null;
                if (enumerator is IDisposable)
                    (enumerator as IDisposable).Dispose();
            }
        }
       
        public bool Ping(string hostNameOrAddress)
        {
            return Ping(hostNameOrAddress, 1000);
        }

       
        public bool Ping(Uri address)
        {
            if (address == null)
                throw new ArgumentNullException();
            return Ping(address.Host, 1000);
        }

       
        public bool Ping(string hostNameOrAddress, int timeout)
        {
            if (!IsAvailable)
                throw new Exception("Network_NetworkNotAvailable");
            return new Ping().Send(hostNameOrAddress, timeout, PingBuffer).Status == IPStatus.Success;
        }

       
        public bool Ping(Uri address, int timeout)
        {
            if (address == null)
                throw new ArgumentNullException();
            return Ping(address.Host, timeout);
        }

        internal void DisconnectListener()
        {
            NetworkChange.NetworkAddressChanged -= OS_NetworkAvailabilityChangedListener;
        }

        private void OS_NetworkAvailabilityChangedListener(object sender, EventArgs e)
        {
            var Expression = m_SyncObject;
            ObjectFlowControl.CheckForSyncLockOnValueType(Expression);
            var lockTaken = false;
            try
            {
                Monitor.Enter(Expression, ref lockTaken);
                m_SynchronizationContext.Post(m_NetworkAvailabilityChangedCallback, null);
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(Expression);
            }
        }

        private void NetworkAvailabilityChangedHandler(object state)
        {
            var isAvailable = IsAvailable;
            if (m_Connected == isAvailable)
                return;
            m_Connected = isAvailable;
            raise_NetworkAvailabilityChanged(this, new NetworkAvailableEventArgs(isAvailable));
        }

        private Uri GetUri(string address)
        {
            return new Uri(address);
        }

        private ICredentials GetNetworkCredentials(string userName, string password)
        {
            if (userName == null)
                userName = "";
            if (password == null)
                password = "";
            return
                !((Operators.CompareString(userName, "", false) == 0) &
                  (Operators.CompareString(password, "", false) == 0))
                    ? new NetworkCredential(userName, password)
                    : null;
        }

        #endregion
    }
}