using System;
using System.Text;
using System.Text.RegularExpressions;
using log4net;

namespace WebServer.Requests
{
    internal class RequestParser
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const int BufferSize = 4096;
        private const string ExtensionUrlBeginsWith = "/api";
        private const string RequestRegularExpression = @"^(?<type>\w+)\s+(?<uri>[^\s\?]+)[^\s]*\s+HTTP/.*|";

        public Request ReadAndParseRequest(ClientStream clientStream)
        {
            if (clientStream == null)
                throw new ArgumentNullException(nameof(clientStream));

            var request = ReadRequest(clientStream);

            var requestMatch = Regex.Match(request, RequestRegularExpression);

            if (requestMatch == Match.Empty)
            {
                return new Request();
            }

            var url = requestMatch.Groups["uri"].Value;
            url = Uri.UnescapeDataString(url);

            var method = Method.CONNECT;
            var methodValue = requestMatch.Groups["type"].Value.ToUpperInvariant();
            if (!string.IsNullOrEmpty(methodValue))
                method = ParseEnum<Method>(methodValue);

            var target = Target.File;
            if (!string.IsNullOrEmpty(url) && url.StartsWith(ExtensionUrlBeginsWith))
            {
                target = Target.Api;
            }

            Log.Info($"Request received: length={request.Length}, url='{url}', method={method}, target={target}");

            return new Request(request.Length, url, method, target);
        }

        private string ReadRequest(ClientStream clientStream)
        {
            var request = string.Empty;
            int requestLength;
            var buffer = new byte[BufferSize];

            while (clientStream.DataAvailable &&
                (requestLength = clientStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                request += Encoding.UTF8.GetString(buffer, 0, requestLength);

                if (request.IndexOf("\r\n\r\n", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    break;
                }
            }
            return request;
        }

        private T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
