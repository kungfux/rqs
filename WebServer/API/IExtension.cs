using System.Net.Sockets;
using WebServer.Requests;

namespace WebServer.API
{
    public interface IExtension
    {
        string Name { get; }
        string AcceptedUrlStartsWith { get; }

        void ProcessRequest(NetworkStream clientStream, Request request);
    }
}
