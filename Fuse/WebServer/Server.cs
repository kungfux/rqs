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
            listener = new TcpListener(ipAddress, port);
        }

        public void Start()
        {
            listener.Start();

            while (!cts.IsCancellationRequested)
            {
                try
                {
                    var clientConnected = listener.AcceptTcpClient();
                    var clientWithToken = Tuple.Create<TcpClient, CancellationTokenSource>(clientConnected, cts);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), clientWithToken);
                }
                catch (SocketException e)
                {
                    if (e.SocketErrorCode == SocketError.Interrupted &&
                        cts.IsCancellationRequested)
                    {
                        // It's okay, user is stopped the server
                    }
                    else
                    {
                        throw;
                    }
                }
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
