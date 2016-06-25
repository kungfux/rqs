using FuseWebServer;
using System;

namespace FuseControlSystem.Models
{
    internal class WebServerModel : IDisposable
    {
        private static readonly Lazy<WebServerModel> _instance = new Lazy<WebServerModel>(() => new WebServerModel());
        public static WebServerModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public readonly Server server = new Server();

        public void Dispose()
        {
            server.Dispose();
        }

        public void StartInstance()
        {
            ReadConfig();
            server.Start();
        }

        public void StopInstance()
        {
            server.Stop();
        }

        public void ReadConfig()
        {
            Config.Instance.Port = Properties.Settings.Default.PORT;
            Config.Instance.RootPath = Properties.Settings.Default.ROOT_PATH;
            Config.Instance.IndexFile = Properties.Settings.Default.INDEX_FILE;
        }
    }
}
