using System;

namespace SkinAssistance.Core.RemoteInfo
{
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
}