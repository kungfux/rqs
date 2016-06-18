using System;

namespace FuseWebServer.WebServer
{
    public class Config
    {
        private static readonly Lazy<Config> _instance = new Lazy<Config>(() => new Config());
        public static Config Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private int _port = 80;
        public int Port
        {
            get { return _port; }
            set 
            {
                if (value >= 1 && value <= 65535)
                    _port = value;
            }
        }

        private string _rootPath = "www";
        public string RootPath
        {
            get { return _rootPath;  }
            set 
            {
                if (value != null && value != string.Empty)
                    _rootPath = value;
            }
        }

        private string _indexFile = "index.html";
        public string IndexFile
        {
            get { return _indexFile; }
            set
            {
                if (value != null && value != string.Empty)
                    _indexFile = value;
            }
        }
    }
}
