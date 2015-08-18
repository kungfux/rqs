using System;

namespace Fuse.WebServer.Request
{
    internal enum RequestType
    {
        CONNECT,
        DELETE,
        GET,
        HEAD,
        OPTIONS,
        PATCH,
        POST,
        PUT,
        TRACE
    }
}
