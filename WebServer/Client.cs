using WebServer.Requests;
using WebServer.Responses;
using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using WebServer.API;

namespace WebServer
{
    internal class Client
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TcpClient _client;
        private readonly NetworkStream _clientStream;
        private static readonly RequestParser Parser = new RequestParser();

        public Client(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            _client = client;
            _clientStream = _client.GetStream();
        }

        public void ProcessRequest(ICollection<IExtension> extensions)
        {
            Request request = Parser.ReadAndParseRequest(_clientStream);

            if (request != null)
            {
                ProcessByTarget(request, extensions);
            }
            else
            {
                Header.Instance.WriteHeader(_clientStream, HttpStatusCode.BadRequest);
            }

            _client.Close();
        }

        private void ProcessByTarget(Request request, ICollection<IExtension> extensions)
        {
            switch(request.Target)
            {
                case Target.File:
                    ProcessTargetFile(request);
                    break;
                case Target.Api:
                    ProcessTargetApi(request, extensions);
                    break;
                default:
                    // TODO: Has no any sense since enum has no another values
                    Header.Instance.WriteHeader(_clientStream, HttpStatusCode.BadRequest);
                    break;
            }
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
                    Header.Instance.WriteHeader(_clientStream, HttpStatusCode.BadRequest);
                    break;
            }
        }

        private void ProcessTargetApi(Request request, ICollection<IExtension> extensions)
        {
            if (extensions != null)
            {
                foreach(IExtension extension in extensions)
                {
                    if (request.Url.StartsWith(extension.AcceptedUrlStartsWith))
                    {
                        try
                        {
                            extension.ProcessRequest(_clientStream, request);
                        }
                        catch (Exception e)
                        {
                            // TODO: Send header???
                            Log.Error("Exception occurs in extension.", e);
                        }
                        return;
                    }
                }
            }

            // If nobody can process the api request
            Header.Instance.WriteHeader(_clientStream, HttpStatusCode.BadRequest);
        }
    }
}
