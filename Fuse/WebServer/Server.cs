using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Fuse.WebServer
{
    internal class Server : IDisposable
    {
        private readonly IPAddress ipAddress = IPAddress.Any;
        private const int port = 80;

        private readonly TcpListener listener;
        private CancellationTokenSource cts;

        public Server()
        {
            listener = new TcpListener(ipAddress, port);
        }

        public void Start()
        {
            if (cts != null && !cts.IsCancellationRequested)
            {
                throw new InvalidOperationException("Server is already started.");
            }
            Task.Run(() => { beginLoop(); });
        }

        public void Stop()
        {
            cts.Cancel();
        }

        public void Dispose()
        {
            cts.Dispose();
            if (listener != null)
            {
                listener.Stop();
            }
        }

        private async void beginLoop()
        {
            cts = new CancellationTokenSource();

            listener.Start();

            while (!cts.IsCancellationRequested)
            {
                if (!listener.Pending())
                {
                    // no pending connections, continue
                    Thread.Sleep(20);
                    continue;
                }

                try
                {
                    // client is awaiting
                    var clientConnected = await listener.AcceptTcpClientAsync();
                    ClientThread(clientConnected);
                }
                catch (SocketException e)
                {
                    if (cts.IsCancellationRequested && e.SocketErrorCode == SocketError.Interrupted)
                    {
                        // cancellation is requested
                        listener.Stop();
                        return;
                    }
                    if (e.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        // remote host breaks the connection
                        continue;
                    }
                    throw;
                }
            }

            listener.Stop();
        }

        private void ClientThread(TcpClient tcpClient)
        {
            Task.Run(() => { new Client(tcpClient); });
        }
    }
}
