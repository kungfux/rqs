using Fuse.WebServer.Request;
using Fuse.WebServer.Response;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Fuse.WebServer
{
    internal class Client
    {
        private readonly TcpClient _client;

        public Client(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");
            _client = client;
        }

        public void ProcessRequest()
        {
            Request.Request request = new Request.Request(_client.GetStream());

            if (request.RequestType != RequestType.GET)
            {
                Header.Instance.WriteHeader(_client.GetStream(), HttpStatusCode.NotImplemented);
                return;
            }

            if (request.RequestSource == RequestSource.FILE)
            {
                FileProcessor.Instance.WriteFile(_client.GetStream(), request.RequestUri);
            }

            _client.Close();
        }
    }
}
