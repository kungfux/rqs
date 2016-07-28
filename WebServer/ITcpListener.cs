using System.Net.Sockets;
using System.Threading.Tasks;

namespace WebServer
{
    internal interface ITcpListener
    {
        void Start();
        void Stop();
        bool Pending();
        Task<TcpClient> AcceptTcpClientAsync();
    }
}
