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

        private readonly CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private void ClientThread(Object pStateInfo)
        {
            var x = (Tuple<TcpClient, CancellationTokenSource>)pStateInfo;
            if (!x.Item2.IsCancellationRequested)
            {
                new Client(x.Item1);
            }
        }

        public Server()
        {
            ThreadPool.SetMaxThreads(maxThreadsCount, maxThreadsCount);
            ThreadPool.SetMinThreads(minThreadsCount, minThreadsCount);
        }

        public void Start()
        {
            _listener.Start();

            while (!cancelTokenSource.IsCancellationRequested)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread),
                    new Tuple<TcpClient, CancellationTokenSource>(_listener.AcceptTcpClient(), cancelTokenSource));
            }
        }

        public void Stop()
        {
            cancelTokenSource.Cancel();
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
