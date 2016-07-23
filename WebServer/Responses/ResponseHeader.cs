using System;
using System.Net;
using log4net;

namespace WebServer.Responses
{
    public class ResponseHeader : Header
    {
        private const string HeaderContentFormat = "Content-Language: en\r\nContent-Type: {0}; charset=utf-8\r\nContent-Length: {1}\r\n";
        private const string HeaderConnectionClose = "Connection: Close\r\n";

        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ResponseHeader(HttpStatusCode httpStatusCode, string contentType = null, long contentLength = 0)
        {
            if (httpStatusCode == HttpStatusCode.MethodNotAllowed || httpStatusCode == HttpStatusCode.NotImplemented)
            {
                var errorMessage = "Response header cannot be send for status " + (int)httpStatusCode;
                _log.Fatal(errorMessage);
                throw new ArgumentException(errorMessage);
            }

            Value = string.Format(
                HeaderGeneralFormat,
                (int) httpStatusCode + " " + httpStatusCode,
                contentType != null ? string.Format(HeaderContentFormat, contentType, contentLength) : HeaderConnectionClose
                );
        }

        public override string Value { get; }
    }
}
