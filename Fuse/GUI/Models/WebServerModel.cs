using Fuse.WebServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fuse.GUI.Models
{
    internal class WebServerModel
    {
        private static readonly Lazy<WebServerModel> _instance = new Lazy<WebServerModel>(() => new WebServerModel());
        public static WebServerModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public bool IsAlive
        {
            get
            {
                return _thread != null ? _thread.IsAlive : false;
            }
        }

        private Thread _thread;
        private Server _serverInstance;
        private Server _server
        {
            get
            {
                if (_serverInstance == null)
                {
                    _serverInstance = new Server();
                }
                return _serverInstance;
            }
        }

        public void StartInstance()
        {
            if (_thread == null)
            {
                _thread = new Thread(_server.Start);
                _thread.Start();
            }
        }

        public void StopInstance()
        {
            if (_thread != null)
            {
                _server.Stop();
                _thread.Abort();
            }
        }
    }
}
