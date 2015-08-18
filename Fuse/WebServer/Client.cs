using Fuse.WebServer.Response;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Fuse.WebServer
{
    internal class Client
    {
        private const string ROOT_PATH = "www";
        private const string INDEX_FILE = "index.html";

        private readonly TcpClient _client;

        public Client(TcpClient client)
        {
            if (client == null)
                throw new ArgumentNullException("client");
            _client = client;
        }

        public void ProcessRequest()
        {
            string request = string.Empty;
            byte[] buffer = new byte[1024];

            int requestLength;
            while ((requestLength = _client.GetStream().Read(buffer, 0, buffer.Length)) > 0)
            {
                request += UTF8Encoding.UTF8.GetString(buffer, 0, requestLength);

                if (request.IndexOf("\r\n\r\n") >= 0 || request.Length > 4096)
                {
                    break;
                }
            }

            Match requestMatch = Regex.Match(request, @"^\w+\s+([^\s\?]+)[^\s]*\s+HTTP/.*|");

            if (requestMatch == Match.Empty)
            {
                Header.Instance.WriteHeader(_client.GetStream(), HttpStatusCode.NotFound);
                return;
            }

            string requestUri = requestMatch.Groups[1].Value;
            requestUri = Uri.UnescapeDataString(requestUri);

            if (requestUri.IndexOf("..") >= 0)
            {
                Header.Instance.WriteHeader(_client.GetStream(), HttpStatusCode.NotFound);
                return;
            }

            if (requestUri.EndsWith("/"))
            {
                requestUri += INDEX_FILE;
            }

            string requestFullPath = ROOT_PATH + "/" + requestUri;

            FileProcessor.Instance.WriteFile(_client.GetStream(), requestFullPath);

            _client.Close();
        }
    }
}
