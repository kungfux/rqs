using System;

namespace FuseWebServer
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
