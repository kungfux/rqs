using System.Net;

namespace WebServer
{
    internal class TcpListener : System.Net.Sockets.TcpListener, ITcpListener
    {
        private static readonly IPAddress _ipAddress = IPAddress.Any;

        public TcpListener(IConfiguration configuration) : base(_ipAddress, configuration.Port)
        {
        }
    }
}
