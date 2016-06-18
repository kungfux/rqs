using log4net;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FuseWebServer.WebServer.Responses
{
    internal class Header
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Lazy<Header> _instance = new Lazy<Header>(() => new Header());
        private const string HEADER_GENERAL_FORMAT = "HTTP/1.1 {0}\nServer: Fuse\nX-Powered-By: Alexander Fuks and contributors\n{1}\n\n";
        private const string HEADER_CONTENT_FORMAT = "Content-Language: en\nContent-Type: {0}; charset=utf-8\nContent-Length: {1}";
        private const string HEADER_ALLOW = "Allow: GET, HEAD, OPTIONS";

        public static Header Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public bool WriteHeader(NetworkStream clientStream, HttpStatusCode httpStatusCode,
            string contentType = null, long contentLength = 0)
        {
            if (httpStatusCode == HttpStatusCode.MethodNotAllowed || httpStatusCode == HttpStatusCode.NotImplemented)
            {
                return SendOptionsHeader(clientStream, httpStatusCode);
            }
            else
            {
                string header =
                    string.Format(HEADER_GENERAL_FORMAT,
                        (int)httpStatusCode + " " + httpStatusCode.ToString(),
                        string.Format(HEADER_CONTENT_FORMAT,
                            contentType,
                            contentLength
                            )
                        );

                return SendHeader(clientStream, header);
            }
        }

        public bool SendOptionsHeader(NetworkStream clientStream, HttpStatusCode httpStatusCode)
        {
            if (httpStatusCode != HttpStatusCode.OK &&
                httpStatusCode != HttpStatusCode.MethodNotAllowed &&
                httpStatusCode != HttpStatusCode.NotImplemented)
            {
                Log.Fatal("OPTIONS header cannot be send for status " + (int)httpStatusCode);
                throw new InvalidOperationException("OPTIONS header should not be sent for statuses 200, 405 or 501");
            }

            string header =
                    string.Format(HEADER_GENERAL_FORMAT,
                    (int)httpStatusCode + " " + httpStatusCode.ToString(),
                    HEADER_ALLOW);

            return SendHeader(clientStream, header);
        }

        private bool SendHeader(NetworkStream clientStream, string header)
        {
            if (string.IsNullOrEmpty(header))
            {
                Log.Fatal("Header cannot be null or empty.");
                throw new ArgumentException("header");
            }

            try
            {
                byte[] headersBuffer = UTF8Encoding.UTF8.GetBytes(header);
                clientStream.Write(headersBuffer, 0, headersBuffer.Length);
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Exception occurs while sending the header to client.", e);
                return false;
            }
        }
    }
}
