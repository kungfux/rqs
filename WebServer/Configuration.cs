using System;

namespace WebServer
{
    public class Configuration
    {
        private static Configuration _instance;
        public static Configuration Instance => _instance ?? (_instance = new Configuration());
        private Configuration() { }

        private int _port = 80;
        private string _rootPath = "www";
        private string _indexFile = "index.html";

        public int Port
        {
            get { return _port; }
            set
            {
                if (value < 1 || value > 65535)
                    throw new InvalidOperationException("Port number cannot be less than 1 or greater than 65535");
                _port = value;
            }
        }      

        public string RootPath
        {
            get { return _rootPath;  }
            set 
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Root path cannot be empty");
                _rootPath = value;
            }
        }

        public string IndexFile
        {
            get { return _indexFile; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new InvalidOperationException("Index file cannot be empty");
                _indexFile = value;
            }
        }
    }
}
