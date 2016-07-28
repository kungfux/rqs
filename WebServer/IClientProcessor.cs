using System.Collections.Generic;
using System.Net.Sockets;
using WebServer.API;

namespace WebServer
{
    internal interface IClientProcessor
    {
        void ProcessRequest(TcpClient client, ICollection<IExtension> extensions);
    }
}
