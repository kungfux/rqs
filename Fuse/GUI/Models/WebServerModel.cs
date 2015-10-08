using Fuse.WebServer;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Fuse.GUI.Models
{
    internal class WebServerModel : INotifyPropertyChanged, IDisposable
    {
        private static readonly Lazy<WebServerModel> _instance = new Lazy<WebServerModel>(() => new WebServerModel());
        public static WebServerModel Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        public WebServerModel()
        {
            server.StatusChanged += server_StatusChanged;
        }

        void server_StatusChanged(object sender, Status e)
        {
            IsAlive = e == Status.Started;
        }

        private readonly Server server = new Server();

        private bool _isAlive;
        public bool IsAlive
        {
            get { return _isAlive; }
            set 
            { 
                _isAlive = value;
                OnProperyChanged();
            }
        }

        public void StartInstance()
        {
            server.Start();
        }

        public void StopInstance()
        {
            server.Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnProperyChanged(string pProperty = "IsAlive")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(pProperty));
            }
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }
}
