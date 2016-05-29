using Fuse.WebServer.API;
using log4net;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Fuse.WebServer
{
    internal class Server : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly IPAddress _ipAddress = IPAddress.Any;
        private readonly int PORT = Config.Instance.Port;

        private readonly TcpListener _listener;
        private CancellationTokenSource _cts;

        private readonly List<IPlugin> plugins = new List<IPlugin>();

        public Server()
        {
            plugins.Add(new Fuse.API.Requirements.RequirementsPlugin());

            try
            {
                _listener = new TcpListener(_ipAddress, PORT);
                Log.Debug(string.Format("Listener initialized on {0}:{1}.", _ipAddress, PORT));
            }
            catch (Exception ex)
            {
                Log.Fatal("Exception occurs while initializing the listener.", ex);
                throw;
            }
        }

        public async void Start()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                Log.Warn("The code tried to start already started listener.");
                throw new InvalidOperationException("Server is already started.");
            }
            await Task.Run(() => { StartClientsAwaiting(); });
        }

        public void Stop()
        {
            if (_cts != null)
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

            try
            {
                _listener.Start();
            }
            catch (Exception ex)
            {
                Log.Fatal("Exception occurs while starting listening the port.", ex);
                throw;
            }

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

                    Log.Info(string.Format("Client {0} ({1}) is connected.", 
                        tcpClient.Client.RemoteEndPoint,
                        Dns.GetHostEntry(
                            IPAddress.Parse(tcpClient.Client.RemoteEndPoint.ToString().
                            Substring(0, tcpClient.Client.RemoteEndPoint.ToString().IndexOf(":")))).HostName));

                    ProcessClient(tcpClient);
                }
                catch (SocketException e)
                {
                    if (_cts.IsCancellationRequested && e.SocketErrorCode == SocketError.Interrupted)
                    {
                        // cancellation is requested
                        _listener.Stop();
                        Log.Debug("Listener stopped as requested.");
                        break;
                    }
                    else if (e.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        // remote host breaks the connection
                        Log.Debug("Connection was reset by client.");
                        continue;
                    }
                    else
                    {
                        Log.Fatal("Exception occurs while accepting the client.", e);
                        throw;
                    }
                }
            }

            _listener.Stop();
            Log.Debug("Listener is stopped.");

            NotifyStatusChanged(Status.Stopped);
        }

        private void ProcessClient(TcpClient tcpClient)
        {
            Task.Run(() => { new Client(tcpClient).ProcessRequest(plugins); });
        }

        private void NotifyStatusChanged(Status status)
        {
            if (StatusChanged != null)
                StatusChanged(this, status);
        }

        public event EventHandler<Status> StatusChanged;
    }
}
