#region NS

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
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

    public class NetWorkConnectInfo
    {
        public string ConnectIpV4 { get; internal set; }

        public string ConnectIpV6 { get; internal set; }

        public string MacAddress { get; internal set; }

        public string AdpaterName { get; internal set; }

        public string DeviceName { get; internal set; }

        public string ConnectName { get; internal set; }
    }

    public class NetWorkInfomation
    {
        public static bool IsConnectd
        {
            get
            {
                var query = new WMIEntityItemsQuery<Win32_IP4RouteTable>();
                var routeTables = query.QueryItem(o => o.Destination == "0.0.0.0");
                return routeTables.Any();
            }
        }
        public static NetWorkConnectInfo ConnectedInfo
        {
            get
            {
                if (IsConnectd)
                {
                    var query = new WMIEntityItemsQuery<Win32_IP4RouteTable>();
                    var routeTables = query.QueryItem(o => o.Destination == "0.0.0.0");
                    var adpaterquery = new WMIEntityItemsQuery<Win32_NetworkAdapter>();
                    var currentConnecttable = adpaterquery.QueryItem(o =>
                        o.InterfaceIndex == routeTables.FirstOrDefault().InterfaceIndex);
                    var currentConnect = currentConnecttable.FirstOrDefault();
                    var netConnectionId = currentConnect.NetConnectionID;
                    var connected = NetworkInterface.GetAllNetworkInterfaces()
                        .FirstOrDefault(o => o.Name == netConnectionId);
                    var iproperties = connected.GetIPProperties().UnicastAddresses;
                    var ipv4 = iproperties.FirstOrDefault(o => o.Address.AddressFamily == AddressFamily.InterNetwork);
                    var ipv6 = iproperties.FirstOrDefault(o => o.Address.AddressFamily == AddressFamily.InterNetworkV6);

                    return new NetWorkConnectInfo
                    {
                        //todo 处理用户只安装一个IP协议. IPV4和IPV6 用户肯定有一个协议会安装
                        ConnectIpV4 = ipv4 == null ? "127.0.0.1" : ipv4.Address.ToString(),
                        ConnectIpV6 = ipv6 == null ? "ff00::/8" : ipv6.Address.ToString(),
                        MacAddress = currentConnect.MACAddress,
                        AdpaterName = currentConnect.PNPDeviceID,
                        DeviceName = currentConnect.Description,
                        ConnectName = netConnectionId
                    };
                }

                return new NetWorkConnectInfo
                {
                    ConnectIpV4 = "127.0.0.1",
                    ConnectIpV6 = "ff00::/8",
                    MacAddress = "00:00:00:00:00:00",
                    AdpaterName = "Unkown",
                    DeviceName = "Unkown"
                };
            }
        }
    }

    public class WMIEntityItemsQuery<T>
    {
        private static readonly Dictionary<string, PropertyInfo> PropertyCache;
        private readonly ManagementClass managementClass;

        static WMIEntityItemsQuery()
        {
            PropertyCache = new Dictionary<string, PropertyInfo>();
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            Array.ForEach(properties, o => { PropertyCache.Add(o.Name, o); });
        }

        public WMIEntityItemsQuery()
        {
            if (WMIClass.Exits<T>(out var className))
                managementClass = new ManagementClass(className);
            else
                throw new Exception("Unkown WMI Class");
        }

        public List<T> QueryItem(Predicate<T> searchFilter)
        {
            var tempItems = new List<T>();
            var collection = managementClass.GetInstances();
            foreach (var c in collection)
            {
                var tobj = Activator.CreateInstance<T>();
                foreach (var property in c.Properties)
                    if (PropertyCache.ContainsKey(property.Name))
                        PropertyCache[property.Name].SetValue(tobj, property.Value);
                if (searchFilter != null)
                {
                    if (searchFilter(tobj))
                        tempItems.Add(tobj);
                }
                else
                {
                    tempItems.Add(tobj);
                }
            }

            return tempItems;
        }

        private class WMIClass
        {
            private static readonly List<string> WMICLASSS = new List<string>();

            static WMIClass()
            {
                var searcher =
                    new ManagementObjectSearcher(new WqlObjectQuery(
                        "select * from meta_class"));
                foreach (ManagementClass wmiClass in
                    searcher.Get())
                    WMICLASSS.Add(
                        wmiClass["__CLASS"].ToString());
            }

            public static bool Exits<T>(out string className)
            {
                className = typeof(T).Name;
                var tempClassName = className;
                return WMICLASSS.Any(o => o == tempClassName);
            }
        }
    }

    public class Win32_IP4RouteTable
    {
        public uint Age { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Destination { get; set; }
        public string Information { get; set; }
        public DateTime InstallDate { get; set; }
        public int InterfaceIndex { get; set; }
        public string Mask { get; set; }
        public int Metric1 { get; set; }
        public int Metric2 { get; set; }
        public int Metric3 { get; set; }
        public int Metric4 { get; set; }
        public int Metric5 { get; set; }
        public string Name { get; set; }
        public string NextHop { get; set; }
        public uint Protocol { get; set; }
        public string Status { get; set; }
        public uint Type { get; set; }
    }

    public class Win32_NetworkAdapter
    {
        public string AdapterType { get; set; }
        public ushort AdapterTypeID { get; set; }
        public bool AutoSense { get; set; }
        public ushort Availability { get; set; }
        public string Caption { get; set; }
        public uint ConfigManagerErrorCode { get; set; }
        public bool ConfigManagerUserConfig { get; set; }
        public string CreationClassName { get; set; }
        public string Description { get; set; }
        public string DeviceID { get; set; }
        public bool ErrorCleared { get; set; }
        private string ErrorDescription { get; set; }
        private string GUID { get; set; }
        public uint Index { get; set; }
        public DateTime InstallDate { get; set; }
        public bool Installed { get; set; }
        public uint InterfaceIndex { get; set; }
        public uint LastErrorCode { get; set; }
        public string MACAddress { get; set; }
        public string Manufacturer { get; set; }
        public uint MaxNumberControlled { get; set; }
        public ulong MaxSpeed { get; set; }
        public string Name { get; set; }
        public string NetConnectionID { get; set; }
        public ushort NetConnectionStatus { get; set; }
        public bool NetEnabled { get; set; }
        public string[] NetworkAddresses { get; set; }
        public string PermanentAddress { get; set; }
        public bool PhysicalAdapter { get; set; }
        public string PNPDeviceID { get; set; }
        public ushort[] PowerManagementCapabilities { get; set; }
        public bool PowerManagementSupported { get; set; }
        public string ProductName { get; set; }
        public string ServiceName { get; set; }
        public ulong Speed { get; set; }
        public string Status { get; set; }
        public ushort StatusInfo { get; set; }
        public string SystemCreationClassName { get; set; }
        public string SystemName { get; set; }
        public string TimeOfLastReset { get; set; }
    }
}