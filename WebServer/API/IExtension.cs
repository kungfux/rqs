using WebServer.Requests;

namespace WebServer.API
{
    public interface IExtension
    {
        string Name { get; }
        string AcceptedUrlStartsWith { get; }

        void ProcessRequest(ClientStream clientStream, Request request);
    }
}
