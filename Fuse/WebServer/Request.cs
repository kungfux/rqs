using System;
using System.Net.Sockets;

namespace Fuse.WebServer
{
    internal class Request
    {
        private readonly TcpClient _client;

        private int _requestLength;
        private string _requestUri;
        private RequestType _requestType;
        private RequestedSourceType _requedtedSourceType;

        public Request(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");
            _client = client;
        }

        public void ParseRequest()
        {

        }
    }
}
