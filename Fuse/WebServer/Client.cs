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

            if (request.Target == Target.FILE)
            {
                switch (request.Method)
                {
                    case Method.HEAD:
                        FileProcessor.Instance.WriteFile(clientStream, request.Url, true);
                        break;
                    case Method.GET:
                        FileProcessor.Instance.WriteFile(clientStream, request.Url);
                        break;
                    case Method.OPTIONS:
                        Header.Instance.SendOptionsHeader(clientStream, HttpStatusCode.OK);
                        break;
                    default:
                        Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotImplemented);
                        break;
                }
            }
            else
            {
                Header.Instance.WriteHeader(clientStream, HttpStatusCode.NotImplemented);
            }

            _client.Close();
        }
    }
}
