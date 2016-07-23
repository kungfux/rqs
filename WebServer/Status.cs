using System;

namespace WebServer
{
    public sealed class Status : EventArgs
    {
        public enum ServerStatus
        {
            Started,
            Stopped
        }
    }
}
