using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using WebServer.API;
using WebServer.Requests;
using WebServer.Responses;

namespace WebServer
{
    internal class ClientProcessor
    {
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly TcpClient _client;
        private readonly ClientStream _clientStream;
        private readonly RequestParser _parser = new RequestParser();

        public ClientProcessor(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            _client = client;
            _clientStream = new ClientStream(_client.GetStream());
        }

        public void ProcessRequest(ICollection<IExtension> extensions)
        {
            Request request = _parser.ReadAndParseRequest(_clientStream);

            if (request != null)
            {
                ProcessByTarget(request, extensions);
            }
            else
            {
                _clientStream.WriteHeader(new ResponseHeader(HttpStatusCode.BadRequest));
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
                    _clientStream.WriteHeader(new ResponseHeader(HttpStatusCode.BadRequest));
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
                    _clientStream.WriteHeader(new OptionsHeader(HttpStatusCode.OK));
                    break;
                default:
                    _clientStream.WriteHeader(new ResponseHeader(HttpStatusCode.BadRequest));
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
                            _log.Error("Exception occurs in extension.", e);
                        }
                        return;
                    }
                }
            }

            // If nobody can process the api request
            _clientStream.WriteHeader(new ResponseHeader(HttpStatusCode.BadRequest));
        }
    }
}
