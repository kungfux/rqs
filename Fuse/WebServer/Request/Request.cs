using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Fuse.WebServer.Request
{
    internal class Request
    {
        private readonly NetworkStream _clientStream;

        private int? _requestLength;
        public int? RequestLength
        {
            get
            {
                return _requestLength ?? 0;
            }
        }

        private string _requestUri;
        public string RequestUri
        {
            get
            {
                return _requestUri ?? "";
            }
        }

        private RequestType _requestType;
        public RequestType RequestType
        {
            get
            {
                return _requestType;
            }
        }

        private RequestSource _requestSource;
        public RequestSource RequestSource
        {
            get
            {
                return _requestSource;
            }
        }

        public Request(NetworkStream clientStream)
        {
            if (clientStream == null)
                throw new ArgumentNullException("clientStream");
            _clientStream = clientStream;

            ParseRequest();
        }

        private void ParseRequest()
        {
            string request = string.Empty;
            byte[] buffer = new byte[4096];

            int requestLength;
            while ((requestLength = _clientStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                request += UTF8Encoding.UTF8.GetString(buffer, 0, requestLength);

                if (request.IndexOf("\r\n\r\n") >= 0)
                {
                    break;
                }
                // TODO: What if request length > 4 Kb?
            }

            Match requestMatch = Regex.Match(request, @"^(?<type>\w+)\s+(?<uri>[^\s\?]+)[^\s]*\s+HTTP/.*|");

            if (requestMatch == Match.Empty)
            {
                return;
            }

            _requestLength = request.Length;
            
            _requestSource = RequestSource.FILE;
            
            _requestUri = requestMatch.Groups["uri"].Value;
            _requestUri = Uri.UnescapeDataString(_requestUri);

            string typeValue = requestMatch.Groups["type"].Value;
            if (!string.IsNullOrEmpty(typeValue))
                _requestType = ParseEnum<RequestType>(typeValue);
        }

        private T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
