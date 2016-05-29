using log4net;
using System;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Fuse.WebServer.Requests
{
    internal class RequestParser
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private const string _apiUrlBeginsWith = "/api";

        public Request ReadAndParseRequest(NetworkStream clientStream)
        {
            if (clientStream == null)
                throw new ArgumentNullException("clientStream");

            string request = string.Empty;
            byte[] buffer = new byte[4096];

            int requestLength;
            while (clientStream.DataAvailable && 
                (requestLength = clientStream.Read(buffer, 0, buffer.Length)) > 0)
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
                return new Request();
            }

            string url = requestMatch.Groups["uri"].Value;
            url = Uri.UnescapeDataString(url);

            Method method = Method.CONNECT;
            string methodValue = requestMatch.Groups["type"].Value;
            if (!string.IsNullOrEmpty(methodValue))
                method = ParseEnum<Method>(methodValue);

            Target target = Target.FILE;
            if (!string.IsNullOrEmpty(url) && url.StartsWith(_apiUrlBeginsWith))
            {
                target = Target.API;
            }

            Log.Info(string.Format("Request received: length={0}, url='{1}', method={2}, target={3}",
                request.Length, url, method, target));

            return new Request(request.Length, url, method, target);
        }

        private T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value);
        }
    }
}
