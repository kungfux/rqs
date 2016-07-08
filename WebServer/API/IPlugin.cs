using System.Net.Sockets;
using WebServer.Requests;

namespace WebServer.API
{
    public interface IPlugin
    {
        string Name { get; }
        string AcceptedUrlStartsWith { get; }

        void ProcessRequest(NetworkStream clientStream, Request request);
    }
}
