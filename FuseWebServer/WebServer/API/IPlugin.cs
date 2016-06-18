using FuseWebServer.WebServer.Requests;
using System.Net.Sockets;

namespace FuseWebServer.WebServer.API
{
    internal interface IPlugin
    {
        string Name { get; }
        string AcceptedUrlStartsWith { get; }

        void ProcessRequest(NetworkStream clientStream, Request request);

        // TODO: Add dependency link?
        // TODO: Move to separate dll?
    }
}
