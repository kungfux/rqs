using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Fuse.WebServer
{
    internal class Server : IDisposable
    {
        private TcpListener _listener;
        private TcpListener Listener
        {
            get
            {
                if (_listener == null)
                {
                    _listener = new TcpListener(IPAddress.Any, 80);
                }
                return _listener;
            }
        }

        private void ClientThread(Object pStateInfo)
        {
            new Client((TcpClient)pStateInfo);
        }

        public void Start()
        {
            Listener.Start();

            while(true)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), Listener.AcceptTcpClient());
            }
        }

        public void Stop()
        {
            Listener.Stop();
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
