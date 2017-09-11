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
    internal class ClientProcessor : IClientProcessor
    {
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly RequestParser _parser = new RequestParser();

        public void ProcessRequest(TcpClient client, ICollection<IExtension> extensions)
        {
            var clientStream = new ClientStream(client.GetStream());
            Request request;

            try
            {
                request = _parser.ReadAndParseRequest(clientStream);
            }
            catch (Exception e)
            {
                _log.Fatal("Unable to parse request", e);
                clientStream.WriteHeader(new ResponseHeader(HttpStatusCode.InternalServerError));
                throw;
            }

            if (request?.Url != null && request.Method != null && request.Target != null)
            {
                ProcessByTarget(request, clientStream, extensions);
            }
            else
            {
                clientStream.WriteHeader(new ResponseHeader(HttpStatusCode.BadRequest));
            }

            client.Close();
        }

        private void ProcessByTarget(Request request, ClientStream clientStream, ICollection<IExtension> extensions)
        {
            switch(request.Target)
            {
                case Target.File:
                    ProcessTargetFile(request, clientStream);
                    break;
                case Target.Api:
                    ProcessTargetApi(request, clientStream, extensions);
                    break;
            }
        }

        private void ProcessTargetFile(Request request, ClientStream clientStream)
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
                    clientStream.WriteHeader(new OptionsHeader(HttpStatusCode.OK));
                    break;
                default:
                    clientStream.WriteHeader(new OptionsHeader(HttpStatusCode.NotImplemented));
                    break;
            }
        }

        private void ProcessTargetApi(Request request, ClientStream clientStream, ICollection<IExtension> extensions)
        {
            if (extensions != null)
            {
                foreach(var extension in extensions)
                {
                    if (request.Url.StartsWith(extension.AcceptedUrlStartsWith))
                    {
                        try
                        {
                            extension.ProcessRequest(clientStream, request);
                        }
                        catch (Exception e)
                        {
                            clientStream.WriteHeader(new ResponseHeader(HttpStatusCode.InternalServerError));
                            _log.Error("Exception occurs in extension.", e);
                        }
                        return;
                    }
                }
            }

            clientStream.WriteHeader(new ResponseHeader(HttpStatusCode.BadRequest));
        }
    }
}
