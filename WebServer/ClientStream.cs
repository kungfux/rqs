using System;
using System.Net.Sockets;
using System.Text;
using log4net;
using WebServer.Responses;

namespace WebServer
{
    public class ClientStream
    {
        private readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly NetworkStream _stream;

        public ClientStream(NetworkStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));
            _stream = stream;
        }

        public bool DataAvailable => _stream.DataAvailable;

        public int Read(byte[] buffer, int offset, int size) => _stream.Read(buffer, offset, size);
        public void Write(byte[] buffer, int offset, int size) => _stream.Write(buffer, offset, size);

        public bool WriteHeader(Header header)
        {
            if (header == null)
                throw new ArgumentNullException(nameof(header));

            try
            {
                byte[] headersBuffer = Encoding.UTF8.GetBytes(header.Value);
                _stream.Write(headersBuffer, 0, headersBuffer.Length);
                return true;
            }
            catch (Exception e)
            {
                _log.Error("Exception occurs while sending the header to client.", e);
                return false;
            }
        }
    }
}
