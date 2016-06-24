using FuseWebServer.WebServer.API;
using FuseWebServer.WebServer.Requests;
using FuseWebServer.WebServer.Responses;
using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace FuseWebServer.WebServer
{
    internal class Client
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

        public void ProcessRequest(ICollection<IPlugin> plugins)
        {
            Request request = parser.ReadAndParseRequest(_clientStream);

            if (request != null)
            {
                ProcessByTarget(request, plugins);
            }
            else
            {
                Header.Instance.WriteHeader(_clientStream, HttpStatusCode.BadRequest);
            }

            _client.Close();
        }

        private void ProcessByTarget(Request request, ICollection<IPlugin> plugins)
        {
            switch(request.Target)
            {
                case Target.FILE:
                    ProcessTargetFile(request);
                    break;
                case Target.API:
                    ProcessTargetApi(request, plugins);
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

        private void ProcessTargetApi(Request request, ICollection<IPlugin> plugins)
        {
            if (plugins != null)
            {
                foreach(IPlugin plugin in plugins)
                {
                    if (request.Url.StartsWith(plugin.AcceptedUrlStartsWith))
                    {
                        try
                        {
                            plugin.ProcessRequest(_clientStream, request);
                        }
                        catch (Exception e)
                        {
                            // TODO: Send header???
                            Log.Error("Exception occurs in plugin.", e);
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
