using System;

namespace SkinAssistance.Core.RemoteInfo
{
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