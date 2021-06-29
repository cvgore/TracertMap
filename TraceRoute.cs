using System.Net;

namespace TracertMap
{
    public class TraceRoute
    {
        public IPAddress IpAddress { get; }
        public int Ping { get; }

        public TraceRoute(IPAddress ipAddress, int ping)
        {
            IpAddress = ipAddress;
            Ping = ping;
        }

        public bool IsIpInternal()
        {
            byte[] bytes = IpAddress.GetAddressBytes();
            switch (bytes[0])
            {
                case 10:
                    return true;
                case 172:
                    return bytes[1] < 32 && bytes[1] >= 16;
                case 192:
                    return bytes[1] == 168;
                default:
                    return false;
            }
        }
    }
}