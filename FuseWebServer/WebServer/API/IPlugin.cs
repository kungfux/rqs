using FuseWebServer.WebServer.Requests;
using System;
using System.Net.Sockets;

namespace FuseWebServer.WebServer.API
{
    public interface IPlugin
    {
        string Name { get; }
        string AcceptedUrlStartsWith { get; }

        void ProcessRequest(NetworkStream clientStream, Request request);
    }
}
