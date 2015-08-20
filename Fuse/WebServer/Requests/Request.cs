using System;

namespace Fuse.WebServer.Requests
{
    internal class Request
    {
        private int? _lenght;
        public int? Length
        {
            get
            {
                return _lenght;
            }
            private set
            {
                _lenght = value;
            }
        }

        private string _url;
        public string Url
        {
            get
            {
                return _url;
            }
            private set
            {
                _url = value;
            }
        }

        private Method _method;
        public Method Method
        {
            get
            {
                return _method;
            }
            private set
            {
                _method = value;
            }
        }

        private Target _target;
        public Target Target
        {
            get
            {
                return _target;
            }
            private set
            {
                _target = value;
            }
        }

        public Request()
        {

        }

        public Request(int? length, string url, Method method, Target target)
        {
            Length = length;
            Url = url;
            Method = method;
            Target = target;
        }
    }
}
