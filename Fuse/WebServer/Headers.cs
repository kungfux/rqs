using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Fuse.WebServer
{
    internal class Headers
    {
        private static readonly Lazy<Headers> _instance = new Lazy<Headers>(() => new Headers());
        
        public static Headers Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public void SendHeader(NetworkStream pClientNetworkStream, HttpStatusCode pHttpStatusCode, 
            string pContentType = null, long pContentLength = 0)
        {
            string header = 
                string.Format("HTTP/1.1 {0}\nContent-Type: {1}\nContent-Length: {2}\n\n",
                (int)pHttpStatusCode + " " + pHttpStatusCode.ToString(),
                pContentType,
                pContentLength);

            byte[] headersBuffer = UTF8Encoding.UTF8.GetBytes(header);

            pClientNetworkStream.Write(headersBuffer, 0, headersBuffer.Length);
        }
    }
}
