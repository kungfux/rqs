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

            var rawRequest = ReadRequest(clientStream);
            var requestMatch = Regex.Match(rawRequest, RequestRegularExpression);

            if (requestMatch == Match.Empty)
            {
                return new Request();
            }

            var request = new Request {Length = rawRequest.Length};

            var url = requestMatch.Groups["uri"].Value;
            url = Uri.UnescapeDataString(url);
            request.Url = url;

            var methodValue = requestMatch.Groups["type"].Value.ToUpperInvariant();
            if (!string.IsNullOrEmpty(methodValue))
            {
                try
                {
                    request.Method = ParseEnum<Method>(methodValue);
                }
                catch (Exception e)
                {
                    Log.Debug($"Cannot convert {methodValue} to Request Method", e);
                }
            }

            request.Target = Target.File;
            if (!string.IsNullOrEmpty(url) && url.StartsWith(ExtensionUrlBeginsWith))
            {
                request.Target = Target.Api;
            }

            Log.Info($"Request received: {request}");

            return request;
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
