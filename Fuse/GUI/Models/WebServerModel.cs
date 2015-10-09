using Fuse.WebServer;
using System;

namespace Fuse.GUI.Models
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
            server.Start();
        }

        public void StopInstance()
        {
            server.Stop();
        }
    }
}
