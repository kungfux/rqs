using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebServer.API;
using static WebServer.Status;

namespace WebServer
{
    internal class Server : IServer
    {
        private const string EXTENSIONS_LOCATION = ".";

        private readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ITcpListener _tcpListener;
        private readonly IClientProcessor _clientProcessor;
        private readonly AutoResetEvent _serverSyncObject;
        private CancellationTokenSource _cts;

        public Server(ITcpListener tcpListener, IClientProcessor clientProcessor)
        {
            _tcpListener = tcpListener;
            _clientProcessor = clientProcessor;
            _serverSyncObject = new AutoResetEvent(true);
        }

        private Lazy<ICollection<IExtension>> Extensions => new Lazy<ICollection<IExtension>>(LoadExtensions);

        public void Start()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                _log.Warn("The code tried to start already started listener.");
                throw new InvalidOperationException("Server is already started.");
            }

            _serverSyncObject.Reset();
            _cts = new CancellationTokenSource();
            try
            {
                _tcpListener.Start();
            }
            catch (Exception ex)
            {
                _log.Fatal("Exception occurs while starting listening the port.", ex);
                throw;
            }
            NotifyStatusChanged(ServerStatus.Started);

            Task.Run(() => { StartClientsAwaiting(); });
        }

        public void Stop()
        {
            _cts?.Cancel();
            if (!_serverSyncObject.WaitOne(TimeSpan.FromSeconds(10)))
            {
                const string error = "Exception occurs while stopping the server.";
                var exception = new TimeoutException(error);
                _log.Fatal(error, exception);
                throw exception;
            }
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _tcpListener?.Stop();
        }

        private async void StartClientsAwaiting()
        {
            while (!_cts.IsCancellationRequested)
            {
                if (!_tcpListener.Pending())
                {
                    // no pending connections, continue
                    await Task.Delay(20);
                    continue;
                }

                try
                {
                    // client is awaiting
                    var tcpClient = await _tcpListener.AcceptTcpClientAsync();

                    _log.Info($"Client {tcpClient.Client.RemoteEndPoint} ({GetResolvedClientName(tcpClient)}) is connected.");

                    ProcessClient(tcpClient);
                }
                catch (SocketException e)
                {
                    if (_cts.IsCancellationRequested && e.SocketErrorCode == SocketError.Interrupted)
                    {
                        // cancellation is requested
                        _tcpListener.Stop();
                        _log.Debug("Listener stopped as requested.");
                        break;
                    }
                    if (e.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        // remote host breaks the connection
                        _log.Debug("Connection was reset by client.");
                        continue;
                    }
                    _log.Fatal("Exception occurs while accepting the client.", e);
                    throw;
                }
            }

            _tcpListener.Stop();
            _log.Debug("Listener is stopped.");

            NotifyStatusChanged(ServerStatus.Stopped);
            _serverSyncObject.Set();
        }

        private void ProcessClient(TcpClient tcpClient)
        {
            Task.Run(() => { _clientProcessor.ProcessRequest(tcpClient, Extensions.Value); });
        }

        public event EventHandler<ServerStatus> StatusChanged;
        private void NotifyStatusChanged(ServerStatus status)
        {
            StatusChanged?.Invoke(this, status);
        }

        private string GetResolvedClientName(TcpClient client)
        {
            var endPoint = (IPEndPoint)client.Client.RemoteEndPoint;
            var ipAddress = endPoint.Address;
            return Dns.GetHostEntry(ipAddress).HostName;
        }

        protected virtual ICollection<IExtension> LoadExtensions()
        {
            var dllFileNames = Directory.GetFiles(EXTENSIONS_LOCATION, "*.dll");
            var assemblies = dllFileNames.Select(AssemblyName.GetAssemblyName).Select(Assembly.Load).ToList();

            var extensionType = typeof(IExtension);
            var extensionTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                if (assembly != null)
                {
                    var types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (!type.IsInterface && !type.IsAbstract && type.GetInterface(extensionType.FullName) != null)
                        {
                            extensionTypes.Add(type);
                        }
                    }
                }
            }

            var extensions = new List<IExtension>(extensionTypes.Count);
            foreach (var type in extensionTypes)
            {
                var extension = (IExtension)Activator.CreateInstance(type);
                extensions.Add(extension);
            }

            return extensions;
        } 
    }
}
