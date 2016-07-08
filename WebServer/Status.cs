using System;

namespace WebServer
{
    public class Status : EventArgs
    {
        public enum ServerStatus
        {
            Started,
            Stopped
        }
    }
}
