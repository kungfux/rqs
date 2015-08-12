using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Fuse.WebServer
{
    internal class Server : IDisposable
    {
        private readonly IPAddress ipAddress = IPAddress.Any;
        private const int port = 80;

        private readonly TcpListener listener;

        private readonly int maxThreadsCount = Environment.ProcessorCount * 4;
        private const int minThreadsCount = 2;

        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        private void ClientThread(object pClientWithToken)
        {
            var clientWithToken = (Tuple<TcpClient, CancellationTokenSource>)pClientWithToken;
            if (!clientWithToken.Item2.IsCancellationRequested)
            {
                new Client(clientWithToken.Item1);
            }
        }

        public Server()
        {
            ThreadPool.SetMaxThreads(maxThreadsCount, maxThreadsCount);
            ThreadPool.SetMinThreads(minThreadsCount, minThreadsCount);

            listener = new TcpListener(ipAddress, port);
        }

        public void Start()
        {
            listener.Start();

            while (!cts.IsCancellationRequested)
            {
                var clientConnected = listener.AcceptTcpClient();
                var clientWithToken = Tuple.Create<TcpClient, CancellationTokenSource>(clientConnected, cts);
                ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), clientWithToken);
            }
        }

        public void Stop()
        {
            cts.Cancel();
            listener.Stop();
        }

        public void Dispose()
        {
            if (listener != null)
            {
                listener.Stop();
            }
        }
    }
}
