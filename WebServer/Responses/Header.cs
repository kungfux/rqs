namespace WebServer.Responses
{
    public abstract class Header
    {
        protected const string HEADER_GENERAL_FORMAT = "HTTP/1.1 {0}\nServer: Fuse\nX-Powered-By: Alexander Fuks and contributors\n{1}\n\n";

        public abstract string Value { get; }
    }
}
