using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WebServer.API;
using static WebServer.Status;

namespace WebServer
{
    public class Server : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly IPAddress IpAddress = IPAddress.Any;
        private readonly int _port = Configuration.Instance.Port;

        private readonly TcpListener _listener;
        private CancellationTokenSource _cts;

        private readonly string EXTENSIONS_LOCATION = ".";
        private readonly ICollection<IExtension> _extensions;

        public Server()
        {
            _extensions = LoadExtensions();

            try
            {
                _listener = new TcpListener(IpAddress, _port);
                Log.Debug($"Listener initialized on {IpAddress}:{_port}.");
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
            _cts?.Cancel();
        }

        public void Dispose()
        {
            _cts?.Dispose();
            _listener?.Stop();
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

            NotifyStatusChanged(ServerStatus.Started);

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

                    Log.Info($"Client {tcpClient.Client.RemoteEndPoint} ({GetResolvedClientName(tcpClient)}) is connected.");

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
                    if (e.SocketErrorCode == SocketError.ConnectionReset)
                    {
                        // remote host breaks the connection
                        Log.Debug("Connection was reset by client.");
                        continue;
                    }
                    Log.Fatal("Exception occurs while accepting the client.", e);
                    throw;
                }
            }

            _listener.Stop();
            Log.Debug("Listener is stopped.");

            NotifyStatusChanged(ServerStatus.Stopped);
        }

        private void ProcessClient(TcpClient tcpClient)
        {
            Task.Run(() => { new ClientProcessor(tcpClient).ProcessRequest(_extensions); });
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

        private ICollection<IExtension> LoadExtensions()
        {
            var dllFileNames = Directory.GetFiles(EXTENSIONS_LOCATION, "*.dll");

            var assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dllFile in dllFileNames)
            {
                var an = AssemblyName.GetAssemblyName(dllFile);
                var assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }

            var extensionType = typeof(IExtension);
            var extensionTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();

                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        if (type.GetInterface(extensionType.FullName) != null)
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
