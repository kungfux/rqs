using FuseWebServer.Requests;
using System;
using System.Net.Sockets;

namespace FuseWebServer.API
{
    public interface IPlugin
    {
        string Name { get; }
        string AcceptedUrlStartsWith { get; }

        void ProcessRequest(NetworkStream clientStream, Request request);
    }
}
