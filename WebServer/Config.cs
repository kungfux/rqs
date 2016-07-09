namespace WebServer
{
    public class Config
    {
        private static Config _instance;
        public static Config Instance => _instance ?? (_instance = new Config());
        private Config() { }

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
                if (!string.IsNullOrEmpty(value))
                    _rootPath = value;
            }
        }

        private string _indexFile = "index.html";
        public string IndexFile
        {
            get { return _indexFile; }
            set
            {
                if (!string.IsNullOrEmpty(value))
                    _indexFile = value;
            }
        }
    }
}
