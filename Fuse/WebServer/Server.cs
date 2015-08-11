using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Fuse.WebServer
{
    internal class Server : IDisposable
    {
        private readonly TcpListener _listener = new TcpListener(IPAddress.Any, 80);

        private readonly int maxThreadsCount = Environment.ProcessorCount * 4;
        private readonly int minThreadsCount = 2;

        private void ClientThread(Object pStateInfo)
        {
            new Client((TcpClient)pStateInfo);
        }

        public Server()
        {
            ThreadPool.SetMaxThreads(maxThreadsCount, maxThreadsCount);
            ThreadPool.SetMinThreads(minThreadsCount, minThreadsCount);
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
