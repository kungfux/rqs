namespace WebServer.Responses
{
    public abstract class Header
    {
        protected const string HEADER_GENERAL_FORMAT = "HTTP/1.1 {0}\r\nServer: Fuse\r\nX-Powered-By: Alexander Fuks and contributors\r\n{1}\r\n\r\n";

        public abstract string Value { get; }
    }
}
