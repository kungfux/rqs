using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Fuse.WebServer
{
    internal class Server : IDisposable
    {
        private readonly TcpListener _listener = new TcpListener(IPAddress.Any, 80);

        private void ClientThread(Object pStateInfo)
        {
            new Client((TcpClient)pStateInfo);
        }

        public void Start()
        {
            _listener.Start();

            while(true)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), _listener.AcceptTcpClient());
            }
        }

        public void Stop()
        {
            _listener.Stop();
        }

        public void Dispose()
        {
            if (_listener != null)
            {
                _listener.Stop();
            }
        }
    }
}
