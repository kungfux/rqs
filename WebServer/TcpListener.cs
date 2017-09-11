using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace WebServer
{
    internal class TcpListener : ITcpListener
    {
        private static readonly IPAddress _ipAddress = IPAddress.Any;
        private readonly System.Net.Sockets.TcpListener _tcpListener;

        public TcpListener(IConfiguration configuration)
        {
            _tcpListener = new System.Net.Sockets.TcpListener(_ipAddress, configuration.Port);
        }

        public void Start() => _tcpListener.Start();

        public void Stop() => _tcpListener.Stop();

        public bool Pending() => _tcpListener.Pending();

        public Task<TcpClient> AcceptTcpClientAsync() => _tcpListener.AcceptTcpClientAsync();
    }
}
