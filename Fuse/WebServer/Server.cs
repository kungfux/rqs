using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Fuse.WebServer
{
    internal class Server : IDisposable
    {
        private static readonly IPAddress _ipAddress = IPAddress.Any;
        private const int PORT = 80;

        private readonly TcpListener _listener;
        private CancellationTokenSource _cts;

        public Server()
        {
            _listener = new TcpListener(_ipAddress, PORT);
        }

        public async void Start()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
                throw new InvalidOperationException("Server is already started.");
            await Task.Run(() => { StartClientsAwaiting(); });
        }

        public void Stop()
        {
            _cts.Cancel();
        }

        public void Dispose()
        {
            if (_cts != null)
                _cts.Dispose();
            if (_listener != null)
                _listener.Stop();
        }

        private async void StartClientsAwaiting()
        {
            _cts = new CancellationTokenSource();

            _listener.Start();

            NotifyStatusChanged(Status.Started);

            while (!_cts.IsCancellationRequested)
            {
                if (!_listener.Pending())
                {
                    // no pending connections, continue
                    await Task.Delay(20);
                    continue;
                }

                try
                {
                    // client is awaiting
                    var tcpClient = await _listener.AcceptTcpClientAsync();
                    ProcessClient(tcpClient);
                }
                catch (SocketException e)
                {
                    if (_cts.IsCancellationRequested && e.SocketErrorCode == SocketError.Interrupted)
                    {
                        // cancellation is requested
                        _listener.Stop();
                        return;
                    }
                    else if (e.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        // remote host breaks the connection
                        continue;
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            _listener.Stop();

            NotifyStatusChanged(Status.Stopped);
        }

        private void ProcessClient(TcpClient tcpClient)
        {
            Task.Run(() => { new Client(tcpClient).ProcessRequest(); });
        }

        private void NotifyStatusChanged(Status status)
        {
            if (StatusChanged != null)
                StatusChanged(this, status);
        }

        public event EventHandler<Status> StatusChanged;
    }
}
