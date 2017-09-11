using System;

namespace WebServer
{
    public interface IServer : IDisposable
    {
        void Start();
        void Stop();
        event EventHandler<Status.ServerStatus> StatusChanged;
    }
}
