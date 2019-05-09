using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SkinAssistance.Core.RemoteInfo
{
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
}