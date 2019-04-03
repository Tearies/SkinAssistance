#region NS

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Devices;
using Microsoft.Win32.SafeHandles;
using SkinAssistance.Core.Native;
using SkinAssistance.Core.Refrecter;
using SkinAssistance.Core.RemoteInfo;
using Timer = System.Timers.Timer;
using UnhandledExceptionEventArgs = Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs;
using UnhandledExceptionEventHandler = Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventHandler;

#endregion
[assembly: AssemblyDistinationVersion]
namespace SkinAssistance.Core.ApplicationService
{
    #region Reference

    #endregion

    [HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
    public class WindowsFormsApplicationBase : ConsoleApplicationBase
    {
        #region Properties

        private readonly WinFormsAppContext m_AppContext;
        private readonly object m_NetworkAvailChangeLock;
        private readonly object m_SplashLock;
        private SynchronizationContext m_AppSyncronizationContext;
        private bool m_DidSplashScreen;
        private bool m_FinishedOnInitilaize;

        [SecurityCritical] private SafeFileHandle m_FirstInstanceMemoryMappedFileHandle;

        private EventWaitHandle m_FirstInstanceSemaphore;
        private string m_MemoryMappedID;
        private EventWaitHandle m_MessageRecievedSemaphore;
        private ArrayList m_NetworkAvailabilityEventHandlers;
        private NetworkInfo m_NetworkObject;
        private bool m_Ok2CloseSplashScreen;
        private bool m_ProcessingUnhandledExceptionEvent;
        private ShutdownMode m_ShutdownStyle;
        private Form m_SplashScreen;
        private Timer m_SplashTimer;
        private bool m_TurnOnNetworkListener;
        private ArrayList m_UnhandledExceptionHandlers;

        public FormCollection OpenForms
        {
            get { return Application.OpenForms; }
        }

        protected Form MainForm
        {
            get
            {
                if (m_AppContext != null) return m_AppContext.MainForm;
                return null;
            }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                if (value == m_SplashScreen)
                    throw new ArgumentException(Utils.GetResourceString("AppModel_SplashAndMainFormTheSame"));
                m_AppContext.MainForm = value;
            }
        }


        public Form SplashScreen
        {
            get { return m_SplashScreen; }
            set
            {
                if (value != null && value == m_AppContext.MainForm)
                    throw new ArgumentException(Utils.GetResourceString("AppModel_SplashAndMainFormTheSame"));
                m_SplashScreen = value;
            }
        }


        public int MinimumSplashScreenDisplayTime { get; set; }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected static bool UseCompatibleTextRendering
        {
            get { return false; }
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ApplicationContext ApplicationContext
        {
            get { return m_AppContext; }
        }


        public bool SaveMySettingsOnExit { get; set; }

        protected internal ShutdownMode ShutdownStyle
        {
            get { return m_ShutdownStyle; }
            set
            {
                ValidateShutdownModeEnumValue(value, "value");
                m_ShutdownStyle = value;
            }
        }


        protected bool EnableVisualStyles { get; set; }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected bool IsSingleInstance { get; set; }

        internal SendOrPostCallback RunNextInstanceDelegate { get; private set; }

        private const string HOST_NAME = "127.0.0.1";
        private const int SECOND_INSTANCE_TIMEOUT = 2500;
        private const int ATTACH_TIMEOUT = 2500;

        #endregion

        #region Method

        public WindowsFormsApplicationBase()
            : this(AuthenticationMode.Windows)
        {
        }


        [SecuritySafeCritical]
        public WindowsFormsApplicationBase(AuthenticationMode authenticationMode)
        {
            MinimumSplashScreenDisplayTime = 2000;
            m_SplashLock = new object();
            m_NetworkAvailChangeLock = new object();
            m_Ok2CloseSplashScreen = true;
            ValidateAuthenticationModeEnumValue(authenticationMode, "authenticationMode");
            if (authenticationMode == AuthenticationMode.Windows)
                try
                {
                    Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                }
                catch (SecurityException ex)
                {
                }
            m_AppContext = new WinFormsAppContext(this);
            new UIPermission(UIPermissionWindow.AllWindows).Assert();
            m_AppSyncronizationContext = AsyncOperationManager.SynchronizationContext;
            AsyncOperationManager.SynchronizationContext = new WindowsFormsSynchronizationContext();
            PermissionSet.RevertAssert();
        }

        public event StartupEventHandler Startup;


        public event StartupNextInstanceEventHandler StartupNextInstance;


        public event ShutdownEventHandler Shutdown;


        public event NetworkAvailableEventHandler NetworkAvailabilityChanged
        {
            add
            {
                var Expression = m_NetworkAvailChangeLock;
                ObjectFlowControl.CheckForSyncLockOnValueType(Expression);
                var lockTaken = false;
                try
                {
                    Monitor.Enter(Expression, ref lockTaken);
                    if (m_NetworkAvailabilityEventHandlers == null)
                        m_NetworkAvailabilityEventHandlers = new ArrayList();
                    m_NetworkAvailabilityEventHandlers.Add(value);
                    m_TurnOnNetworkListener = true;
                    if (!((m_NetworkObject == null) & m_FinishedOnInitilaize))
                        return;
                    m_NetworkObject = new NetworkInfo();
                    m_NetworkObject.NetworkAvailabilityChanged += NetworkAvailableEventAdaptor;
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
                m_NetworkObject.NetworkAvailabilityChanged -= NetworkAvailableEventAdaptor;
                if (m_NetworkObject == null)
                    return;
                m_NetworkObject.DisconnectListener();
                m_NetworkObject = null;
            }
        }


        public event UnhandledExceptionEventHandler UnhandledException
        {
            add
            {
                if (m_UnhandledExceptionHandlers == null)
                    m_UnhandledExceptionHandlers = new ArrayList();
                m_UnhandledExceptionHandlers.Add(value);
                if (m_UnhandledExceptionHandlers.Count != 1)
                    return;
                Application.ThreadException += OnUnhandledExceptionEventAdaptor;
            }
            remove
            {
                if (m_UnhandledExceptionHandlers == null || m_UnhandledExceptionHandlers.Count <= 0)
                    return;
                m_UnhandledExceptionHandlers.Remove(value);
                if (m_UnhandledExceptionHandlers.Count != 0)
                    return;
                Application.ThreadException -= OnUnhandledExceptionEventAdaptor;
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
                    try
                    {
                        if (availableEventHandler != null)
                            availableEventHandler(sender, e);
                    }
                    catch (Exception ex)
                    {
                        if (!OnUnhandledException(new UnhandledExceptionEventArgs(true, ex)))
                            throw;
                    }
            }
            finally
            {
                IEnumerator enumerator = null;
                if (enumerator is IDisposable)
                    (enumerator as IDisposable).Dispose();
            }
        }

        [SpecialName]
        private void raise_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (m_UnhandledExceptionHandlers == null)
                return;
            m_ProcessingUnhandledExceptionEvent = true;
            try
            {
                foreach (UnhandledExceptionEventHandler exceptionEventHandler in m_UnhandledExceptionHandlers)
                    if (exceptionEventHandler != null)
                        exceptionEventHandler(sender, e);
            }
            finally
            {
                IEnumerator enumerator = null;
                if (enumerator is IDisposable)
                    (enumerator as IDisposable).Dispose();
            }
            m_ProcessingUnhandledExceptionEvent = false;
        }


        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Run(string[] commandLine)
        {
            InternalCommandLine = new ReadOnlyCollection<string>(commandLine);
            if (!IsSingleInstance)
            {
                DoApplicationModel();
            }
            else
            {
                var applicationInstanceId = GetApplicationInstanceID();
                m_MemoryMappedID = applicationInstanceId + "Map";
                var name1 = applicationInstanceId + "Event";
                var name2 = applicationInstanceId + "Event2";
                RunNextInstanceDelegate = OnStartupNextInstanceMarshallingAdaptor;
                new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Assert();
                var name3 = WindowsIdentity.GetCurrent().Name;
                var ChannelIsSecure = (uint)Operators.CompareString(name3, "", false) > 0U;
                CodeAccessPermission.RevertAssert();
                bool createdNew1;
                if (ChannelIsSecure)
                {
                    var rule = new EventWaitHandleAccessRule(name3, EventWaitHandleRights.FullControl,
                        AccessControlType.Allow);
                    var eventSecurity1 = new EventWaitHandleSecurity();
                    new SecurityPermission(SecurityPermissionFlag.ControlPrincipal).Assert();
                    eventSecurity1.AddAccessRule(rule);
                    CodeAccessPermission.RevertAssert();
                    m_FirstInstanceSemaphore = new EventWaitHandle(false, EventResetMode.ManualReset, name1,
                        out createdNew1, eventSecurity1);
                    var num1 = 0;
                    var num2 = 0;
                    var name4 = name2;
                    var flag = false;
                    // ISSUE: explicit reference operation
                    // ISSUE: variable of a reference type
                    var createdNew2 = flag;
                    var eventSecurity2 = eventSecurity1;
                    m_MessageRecievedSemaphore = new EventWaitHandle(num1 != 0, (EventResetMode)num2, name4,
                        out createdNew2, eventSecurity2);
                }
                else
                {
                    m_FirstInstanceSemaphore = new EventWaitHandle(false, EventResetMode.ManualReset, name1,
                        out createdNew1);
                    m_MessageRecievedSemaphore = new EventWaitHandle(false, EventResetMode.AutoReset, name2);
                }
                if (createdNew1)
                {
                    try
                    {
                        var tcpServerChannel = (TcpServerChannel)RegisterChannel(ChannelType.Server, ChannelIsSecure);
                        var remoteCommunicator = new RemoteCommunicator(this, m_MessageRecievedSemaphore);
                        var str = applicationInstanceId + ".rem";
                        new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration).Assert();
                        var URI = str;
                        RemotingServices.Marshal(remoteCommunicator, URI);
                        CodeAccessPermission.RevertAssert();
                        var objectUri = str;
                        WriteUrlToMemoryMappedFile(tcpServerChannel.GetUrlsForUri(objectUri)[0]);
                        m_FirstInstanceSemaphore.Set();
                        DoApplicationModel();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (m_MessageRecievedSemaphore != null)
                            m_MessageRecievedSemaphore.Close();
                        if (m_FirstInstanceSemaphore != null)
                            m_FirstInstanceSemaphore.Close();
                        if (m_FirstInstanceMemoryMappedFileHandle != null && !m_FirstInstanceMemoryMappedFileHandle.IsInvalid)
                            m_FirstInstanceMemoryMappedFileHandle.Close();
                    }
                }
                else
                {
                    if (!m_FirstInstanceSemaphore.WaitOne(2500, false))
                        throw new CantStartSingleInstanceException();
                    RegisterChannel(ChannelType.Client, ChannelIsSecure);
                    var url = ReadUrlFromMemoryMappedFile();
                    if (url == null)
                        throw new CantStartSingleInstanceException();
                    var remoteCommunicator =
                        (RemoteCommunicator)RemotingServices.Connect(typeof(RemoteCommunicator), url);
                    var permissionSet = new PermissionSet(PermissionState.None);
                    var securityPermission =
                        new SecurityPermission(SecurityPermissionFlag.UnmanagedCode |
                                               SecurityPermissionFlag.SerializationFormatter |
                                               SecurityPermissionFlag.ControlPrincipal);
                    permissionSet.AddPermission(securityPermission);
                    var dnsPermission = new DnsPermission(PermissionState.Unrestricted);
                    permissionSet.AddPermission(dnsPermission);
                    var socketPermission = new SocketPermission(NetworkAccess.Connect, TransportType.Tcp, "127.0.0.1",
                        -1);
                    permissionSet.AddPermission(socketPermission);
                    var environmentPermission = new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME");
                    permissionSet.AddPermission(environmentPermission);
                    permissionSet.Assert();
                    var commandLineArgs = CommandLineArgs;
                    remoteCommunicator.RunNextInstance(commandLineArgs);
                    PermissionSet.RevertAssert();
                    if (!m_MessageRecievedSemaphore.WaitOne(2500, false))
                        throw new CantStartSingleInstanceException();
                }
            }
        }


        public void DoEvents()
        {
            Application.DoEvents();
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [STAThread]
        protected virtual bool OnInitialize(ReadOnlyCollection<string> commandLineArgs)
        {
            if (EnableVisualStyles)
                Application.EnableVisualStyles();
            if (!commandLineArgs.Contains("/nosplash") && !CommandLineArgs.Contains("-nosplash"))
                ShowSplashScreen();
            m_FinishedOnInitilaize = true;
            return true;
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual bool OnStartup(StartupEventArgs eventArgs)
        {
            eventArgs.Cancel = false;
            if (m_TurnOnNetworkListener & (m_NetworkObject == null))
            {
                m_NetworkObject = new NetworkInfo();
                m_NetworkObject.NetworkAvailabilityChanged += NetworkAvailableEventAdaptor;
            }
            // ISSUE: reference to a compiler-generated field
            var startupEventHandler = Startup;
            if (startupEventHandler != null)
                startupEventHandler(this, eventArgs);
            return !eventArgs.Cancel;
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SecuritySafeCritical]
        protected virtual void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
        {
            var instanceEventHandler = StartupNextInstance;
            if (instanceEventHandler != null)
                instanceEventHandler(this, eventArgs);
            new UIPermission(UIPermissionWindow.AllWindows).Assert();
            if (!eventArgs.BringToForeground || MainForm == null)
                return;
            if (MainForm.WindowState == FormWindowState.Minimized)
                MainForm.WindowState = FormWindowState.Normal;
            MainForm.Activate();
        }


        [SecuritySafeCritical]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnRun()
        {
            if (MainForm == null)
            {
                OnCreateMainForm();
                if (MainForm == null)
                    throw new NoStartupFormException();
                MainForm.Load += MainFormLoadingDone;
            }
            try
            {
                Application.Run(m_AppContext);
            }
            finally
            {
                if (m_NetworkObject != null)
                    m_NetworkObject.DisconnectListener();
                if (m_FirstInstanceSemaphore != null)
                {
                    m_FirstInstanceSemaphore.Close();
                    m_FirstInstanceSemaphore = null;
                }
                AsyncOperationManager.SynchronizationContext = m_AppSyncronizationContext;
                m_AppSyncronizationContext = null;
            }
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnCreateSplashScreen()
        {
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnCreateMainForm()
        {
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual void OnShutdown()
        {
            var shutdownEventHandler = Shutdown;
            if (shutdownEventHandler == null)
                return;
            shutdownEventHandler(this, EventArgs.Empty);
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected virtual bool OnUnhandledException(UnhandledExceptionEventArgs e)
        {
            bool flag;
            if (m_UnhandledExceptionHandlers != null && m_UnhandledExceptionHandlers.Count > 0)
            {
                raise_UnhandledException(this, e);
                if (e.ExitApplication)
                    Application.Exit();
                flag = true;
            }
            else
            {
                flag = false;
            }
            return flag;
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected void ShowSplashScreen()
        {
            if (m_DidSplashScreen)
                return;
            m_DidSplashScreen = true;
            if (m_SplashScreen == null)
                OnCreateSplashScreen();
            if (m_SplashScreen == null)
                return;
            if (MinimumSplashScreenDisplayTime > 0)
            {
                m_Ok2CloseSplashScreen = false;
                m_SplashTimer = new Timer(MinimumSplashScreenDisplayTime);
                m_SplashTimer.Elapsed += MinimumSplashExposureTimeIsUp;
                m_SplashTimer.AutoReset = false;
            }
            else
            {
                m_Ok2CloseSplashScreen = true;
            }
            new Thread(DisplaySplash).Start();
        }


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [SecuritySafeCritical]
        protected void HideSplashScreen()
        {
            var Expression = m_SplashLock;
            ObjectFlowControl.CheckForSyncLockOnValueType(Expression);
            var lockTaken = false;
            try
            {
                Monitor.Enter(Expression, ref lockTaken);
                if (MainForm != null)
                {
                    new UIPermission(UIPermissionWindow.AllWindows).Assert();
                    MainForm.Activate();
                    PermissionSet.RevertAssert();
                }
                if (m_SplashScreen == null || m_SplashScreen.IsDisposed)
                    return;
                m_SplashScreen.Invoke(new DisposeDelegate(((Component)m_SplashScreen).Dispose));
                m_SplashScreen = null;
            }
            finally
            {
                if (lockTaken)
                    Monitor.Exit(Expression);
            }
        }

        private void ValidateAuthenticationModeEnumValue(AuthenticationMode value, string paramName)
        {
            if (value < AuthenticationMode.Windows || value > AuthenticationMode.ApplicationDefined)
                throw new InvalidEnumArgumentException(paramName, (int)value, typeof(AuthenticationMode));
        }

        private void ValidateShutdownModeEnumValue(ShutdownMode value, string paramName)
        {
            if (value < ShutdownMode.AfterMainFormCloses || value > ShutdownMode.AfterAllFormsClose)
                throw new InvalidEnumArgumentException(paramName, (int)value, typeof(ShutdownMode));
        }

        private void DisplaySplash()
        {
            if (m_SplashTimer != null)
                m_SplashTimer.Enabled = true;
            Application.Run(m_SplashScreen);
        }

        private void MinimumSplashExposureTimeIsUp(object sender, ElapsedEventArgs e)
        {
            if (m_SplashTimer != null)
            {
                m_SplashTimer.Dispose();
                m_SplashTimer = null;
            }
            m_Ok2CloseSplashScreen = true;
        }

        private void MainFormLoadingDone(object sender, EventArgs e)
        {
            MainForm.Load -= MainFormLoadingDone;
            while (!m_Ok2CloseSplashScreen)
                DoEvents();
            HideSplashScreen();
        }

        private void OnUnhandledExceptionEventAdaptor(object sender, ThreadExceptionEventArgs e)
        {
            OnUnhandledException(new UnhandledExceptionEventArgs(true, e.Exception));
        }

        private void OnStartupNextInstanceMarshallingAdaptor(object args)
        {
            OnStartupNextInstance(new StartupNextInstanceEventArgs((ReadOnlyCollection<string>)args, true));
        }

        private void NetworkAvailableEventAdaptor(object sender, NetworkAvailableEventArgs e)
        {
            raise_NetworkAvailabilityChanged(sender, e);
        }

        [SecurityCritical]
        private string ReadUrlFromMemoryMappedFile()
        {
            string str1;
            string str2;
            using (var safeFileHandle = UnsafeNativeMethods.OpenFileMapping(4, false, m_MemoryMappedID))
            {
                if (safeFileHandle.IsInvalid)
                {
                    str1 = null;
                    goto label_14;
                }
                using (
                    var viewOfFileHandle = UnsafeNativeMethods.MapViewOfFile(safeFileHandle.DangerousGetHandle(), 4, 0,
                        0, UIntPtr.Zero))
                {
                    if (viewOfFileHandle.IsInvalid)
                        throw new Win32Exception("AppModel_CantGetMemoryMappedFile");
                    str2 = Marshal.PtrToStringUni(viewOfFileHandle.DangerousGetHandle());
                }
            }
            str1 = str2;
            label_14:
            return str1;
        }

        [SecurityCritical]
        private void WriteUrlToMemoryMappedFile(string URL)
        {
            var hFile = new HandleRef(null, new IntPtr(-1));
            using (var lpAttributes = new NativeTypes.SECURITY_ATTRIBUTES())
            {
                lpAttributes.bInheritHandle = false;
                var flag = false;
                try
                {
                    new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Assert();
                    flag =
                        NativeMethods.ConvertStringSecurityDescriptorToSecurityDescriptor("D:(A;;GA;;;CO)(A;;GR;;;AU)",
                            1U, ref lpAttributes.lpSecurityDescriptor, IntPtr.Zero);
                    CodeAccessPermission.RevertAssert();
                }
                catch (EntryPointNotFoundException ex)
                {
                    lpAttributes.lpSecurityDescriptor = IntPtr.Zero;
                }
                catch (DllNotFoundException ex)
                {
                    lpAttributes.lpSecurityDescriptor = IntPtr.Zero;
                }
                if (!flag)
                    lpAttributes.lpSecurityDescriptor = IntPtr.Zero;
                m_FirstInstanceMemoryMappedFileHandle = UnsafeNativeMethods.CreateFileMapping(hFile, lpAttributes, 4, 0,
                    checked(URL.Length + 1 * 2), m_MemoryMappedID);
                if (m_FirstInstanceMemoryMappedFileHandle.IsInvalid)
                    throw new Win32Exception("AppModel_CantGetMemoryMappedFile SECURITY_ATTRIBUTES");
            }
            using (
                var viewOfFileHandle =
                    UnsafeNativeMethods.MapViewOfFile(m_FirstInstanceMemoryMappedFileHandle.DangerousGetHandle(), 2, 0,
                        0, UIntPtr.Zero))
            {
                if (viewOfFileHandle.IsInvalid)
                    throw new Win32Exception("AppModel_CantGetMemoryMappedFile SetData");
                var source = URL.ToCharArray();
                Marshal.Copy(source, 0, viewOfFileHandle.DangerousGetHandle(), source.Length);
            }
        }

        [SecurityCritical]
        private IChannel RegisterChannel(ChannelType channelType, bool channelIsSecure)
        {
            var permissionSet = new PermissionSet(PermissionState.None);
            var securityPermission1 =
                new SecurityPermission(SecurityPermissionFlag.UnmanagedCode |
                                       SecurityPermissionFlag.SerializationFormatter |
                                       SecurityPermissionFlag.ControlPrincipal);
            permissionSet.AddPermission(securityPermission1);
            var socketPermission = new SocketPermission(NetworkAccess.Accept, TransportType.Tcp, "127.0.0.1", 0);
            permissionSet.AddPermission(socketPermission);
            var environmentPermission = new EnvironmentPermission(EnvironmentPermissionAccess.Read, "USERNAME");
            permissionSet.AddPermission(environmentPermission);
            var securityPermission2 = new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration);
            permissionSet.AddPermission(securityPermission2);
            permissionSet.Assert();
            IDictionary properties = new Hashtable(3);
            properties.Add("bindTo", "127.0.0.1");
            properties.Add("port", 0);
            properties.Add("name", string.Empty);
            if (channelIsSecure)
            {
                properties.Add("secure", true);
                properties.Add("tokenimpersonationlevel", TokenImpersonationLevel.Impersonation);
                properties.Add("impersonate", true);
            }
            var chnl = channelType != ChannelType.Server
                ? new TcpClientChannel(properties, null)
                : (IChannel)new TcpServerChannel(properties, null);
            ChannelServices.RegisterChannel(chnl, channelIsSecure);
            PermissionSet.RevertAssert();
            return chnl;
        }

        private void DoApplicationModel()
        {
            var eventArgs = new StartupEventArgs(CommandLineArgs);
            if (!Debugger.IsAttached)
            {
                try
                {
                    if (!OnInitialize(CommandLineArgs) || !OnStartup(eventArgs))
                        return;
                    OnRun();
                    OnShutdown();
                }
                catch (Exception ex)
                {
                    if (m_ProcessingUnhandledExceptionEvent)
                        throw;
                    if (OnUnhandledException(new UnhandledExceptionEventArgs(true, ex)))
                        return;
                    throw;
                }
            }
            else
            {
                if (!OnInitialize(CommandLineArgs) || !OnStartup(eventArgs))
                    return;
                OnRun();
                OnShutdown();
            }
        }

        [SecurityCritical]
        private string GetApplicationInstanceID()
        {
            var permissionSet = new PermissionSet(PermissionState.None);
            var fileIoPermission = new FileIOPermission(PermissionState.Unrestricted);
            permissionSet.AddPermission(fileIoPermission);
            var securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
            permissionSet.AddPermission(securityPermission);
            permissionSet.Assert();
            var libGuidForAssembly = ProductInfo.ProductId;
            var strArray = ProductInfo.Version.ToString().Split(Conversions.ToCharArrayRankOne("."));
            PermissionSet.RevertAssert();
            return libGuidForAssembly + strArray[0] + "." + strArray[1];
        }

        #endregion
    }
}