using System;
using System.Net.Sockets;
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

        public Request ReadAndParseRequest(NetworkStream clientStream)
        {
            if (clientStream == null)
                throw new ArgumentNullException(nameof(clientStream));

            string request = string.Empty;
            byte[] buffer = new byte[BufferSize];

            int requestLength;
            while (clientStream.DataAvailable && 
                (requestLength = clientStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                request += Encoding.UTF8.GetString(buffer, 0, requestLength);

                if (request.IndexOf("\r\n\r\n", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    break;
                }
            }

            Match requestMatch = Regex.Match(request, @"^(?<type>\w+)\s+(?<uri>[^\s\?]+)[^\s]*\s+HTTP/.*|");

            if (requestMatch == Match.Empty)
            {
                return new Request();
            }

            string url = requestMatch.Groups["uri"].Value;
            url = Uri.UnescapeDataString(url);

            Method method = Method.CONNECT;
            string methodValue = requestMatch.Groups["type"].Value.ToUpperInvariant();
            if (!string.IsNullOrEmpty(methodValue))
                method = ParseEnum<Method>(methodValue);

            Target target = Target.File;
            if (!string.IsNullOrEmpty(url) && url.StartsWith(ExtensionUrlBeginsWith))
            {
                target = Target.Api;
            }

            Log.Info($"Request received: length={request.Length}, url='{url}', method={method}, target={target}");

            return new Request(request.Length, url, method, target);
        }

        private T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
