using Fuse.WebServer.Requests;
using Fuse.WebServer.Responses;
using System;
using System.Net;
using System.Net.Sockets;

namespace Fuse.WebServer
{
    internal class Client
    {
        private readonly TcpClient _client;
        private static readonly RequestParser parser = new RequestParser();

        public Client(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");
            _client = client;
        }

        public void ProcessRequest()
        {
            NetworkStream clientStream = _client.GetStream();

            Request request = parser.ReadAndParseRequest(clientStream);

            if (request.Method != Method.GET)
            {
                Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotImplemented);
                return;
            }

            if (request.Target == Target.FILE)
            {
                FileProcessor.Instance.WriteFile(clientStream, request.Url);
            }

            _client.Close();
        }
    }
}
