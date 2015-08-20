using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Fuse.WebServer.Responses
{
    internal class Header
    {
        private static readonly Lazy<Header> _instance = new Lazy<Header>(() => new Header());
        private const string HEADER_FORMAT = "HTTP/1.1 {0}\nContent-Type: {1}\nContent-Length: {2}\n\n";
        
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
            string header = 
                string.Format(HEADER_FORMAT,
                (int)httpStatusCode + " " + httpStatusCode.ToString(),
                contentType,
                contentLength);

            try
            {
                byte[] headersBuffer = UTF8Encoding.UTF8.GetBytes(header);
                clientStream.Write(headersBuffer, 0, headersBuffer.Length);
                return true;
            }
            catch(Exception e)
            {
                // TODO: Log exception
                return false;
            }
        }
    }
}
