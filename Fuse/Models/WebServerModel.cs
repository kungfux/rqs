using WebServer;
using System;

namespace Fuse.Models
{
    internal class WebServerModel : IDisposable
    {
        private static WebServerModel _instance;
        public static WebServerModel Instance => _instance ?? (_instance = new WebServerModel());
        private WebServerModel() { }

        public readonly Server Server = new Server();

        public void Dispose()
        {
            Server.Dispose();
        }

        public void StartInstance()
        {
            ReadConfig();
            Server.Start();
        }

        public void StopInstance()
        {
            Server.Stop();
        }

        public void ReadConfig()
        {
            Configuration.Instance.Port = Properties.Settings.Default.PORT;
            Configuration.Instance.RootPath = Properties.Settings.Default.ROOT_PATH;
            Configuration.Instance.IndexFile = Properties.Settings.Default.INDEX_FILE;
        }
    }
}
