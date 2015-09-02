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
        private readonly NetworkStream _clientStream;
        private static readonly RequestParser parser = new RequestParser();

        public Client(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");
            _client = client;
            _clientStream = _client.GetStream();
        }

        public void ProcessRequest()
        {
            Request request = parser.ReadAndParseRequest(_clientStream);

            if (request != null)
            {
                ProcessByTarget(request);
            }
            else
            {
                Header.Instance.WriteHeader(_clientStream, HttpStatusCode.BadRequest);
            }

            _client.Close();
        }

        private void ProcessByTarget(Request request)
        {
            switch(request.Target)
            {
                case Target.FILE:
                    ProcessTargetFile(request);
                    break;
                default:
                    ProcessAsNotImplemented();
                    break;
            }
        }

        private void ProcessAsNotImplemented()
        {
            Header.Instance.WriteHeader(_clientStream, HttpStatusCode.NotImplemented);
        }

        private void ProcessTargetFile(Request request)
        {
            switch (request.Method)
            {
                case Method.HEAD:
                    FileProcessor.Instance.WriteFile(_clientStream, request.Url, true);
                    break;
                case Method.GET:
                    FileProcessor.Instance.WriteFile(_clientStream, request.Url);
                    break;
                case Method.OPTIONS:
                    Header.Instance.SendOptionsHeader(_clientStream, HttpStatusCode.OK);
                    break;
                default:
                    ProcessAsNotImplemented();
                    break;
            }
        }
    }
}
