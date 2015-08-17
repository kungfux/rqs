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
        private const string cRootPath = "www";
        private const string cIndexFile = "index.html";

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
                Headers.Instance.SendHeader(_client.GetStream(), HttpStatusCode.NotFound);
                return;
            }

            string requestUri = requestMatch.Groups[1].Value;
            requestUri = Uri.UnescapeDataString(requestUri);

            if (requestUri.IndexOf("..") >= 0)
            {
                Headers.Instance.SendHeader(_client.GetStream(), HttpStatusCode.NotFound);
                return;
            }

            if (requestUri.EndsWith("/"))
            {
                requestUri += cIndexFile;
            }

            string requestFullPath = cRootPath + "/" + requestUri;

            if (!File.Exists(requestFullPath))
            {
                Headers.Instance.SendHeader(_client.GetStream(), HttpStatusCode.NotFound);
                return;
            }

            string extension = requestUri.Substring(requestUri.LastIndexOf('.'));
            string contentType = ContentType.Instance.ContentTypeByExtension(extension);

            FileStream fileStream;
            try
            {
                fileStream = new FileStream(requestFullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch (Exception)
            {
                Headers.Instance.SendHeader(_client.GetStream(), HttpStatusCode.InternalServerError);
                return;
            }

            Headers.Instance.SendHeader(_client.GetStream(), HttpStatusCode.OK, contentType, fileStream.Length);

            int responceLength;
            byte[] responceBuffer = new byte[1024];

            while (fileStream.Position < fileStream.Length)
            {
                responceLength = fileStream.Read(responceBuffer, 0, responceBuffer.Length);
                _client.GetStream().Write(responceBuffer, 0, responceLength);
            }

            fileStream.Close();
            _client.Close();
        }
    }
}
