using System;

namespace WebServer.Responses
{
    public abstract class Header
    {
        protected string HeaderGeneralFormat => 
            "HTTP/1.1 {0}\r\n" + 
            $"Date: {DateTime.UtcNow.ToString("R")}\r\n" +  
            "Server: Fuse\r\n" +
            "X-Powered-By: Alexander Fuks and contributors\r\n" +
            "{1}" + 
            "\r\n\r\n";

        public abstract string Value { get; }
    }
}
