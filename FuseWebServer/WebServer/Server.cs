using FuseWebServer.WebServer.API;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace FuseWebServer.WebServer
{
    public class Server : IDisposable
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly IPAddress _ipAddress = IPAddress.Any;
        private readonly int PORT = Config.Instance.Port;

        private readonly TcpListener _listener;
        private CancellationTokenSource _cts;

        private readonly string PLUGINS_LOCATION = ".";
        private readonly ICollection<IPlugin> plugins;

        public Server()
        {
            plugins = LoadPlugins();

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

        public event EventHandler<Status> StatusChanged;
        private void NotifyStatusChanged(Status status)
        {
            StatusChanged?.Invoke(this, status);
        }

        private ICollection<IPlugin> LoadPlugins()
        {
            string[] dllFileNames = null;
            dllFileNames = Directory.GetFiles(PLUGINS_LOCATION, "*.dll");

            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dllFile in dllFileNames)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }

            Type pluginType = typeof(IPlugin);
            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
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
                        else
                        {
                            if (type.GetInterface(pluginType.FullName) != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }

            ICollection<IPlugin> plugins = new List<IPlugin>(pluginTypes.Count);
            foreach (Type type in pluginTypes)
            {
                IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                plugins.Add(plugin);
            }

            return plugins;
        } 
    }
}
