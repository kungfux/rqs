using Fuse.WebServer.API;
using System;
using Fuse.WebServer.Requests;
using log4net;
using Fuse.WebServer.Responses;
using System.Net.Sockets;

namespace Fuse.API.Requirements
{
    internal class RequirementsPlugin : IPlugin
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Name
        {
            get { return "Requirements Search"; }
        }

        public string AcceptedUrlStartsWith
        {
            //  According to: Web API Design - Crafting Interfaces that Developers Love

            // /requirement/
            //  GET     List
            //  POST    Create new
            //  PUT     Bulk update
            //  DELETE  Delete

            // /requirement/FR1
            //  GET     Show FR1
            //  POST    Error
            //  PUT     Update if exists
            //  DELETE  Delete FR1

            // TODO: We need here: Url, document type?, method?
            // TODO: How to catch both v1 and v2 api requests by the same module?
            get { return "/api/v1/requirement/"; }
        }

        // TODO: Isolate plugins from accessing web server objects
        public void ProcessRequest(NetworkStream clientStream, Request request)
        {
            Log.Debug("Request for API is received.");

            byte[] response = GetBytes("<html><h1>Requirement Search</h1><p>Here is your requirement.</p></html>");

            Header.Instance.WriteHeader(clientStream, System.Net.HttpStatusCode.OK, "text/html", response.Length);
            clientStream.Write(response, 0, response.Length);
        }

        private byte[] GetBytes(string value)
        {
            byte[] bytes = new byte[value.Length * sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
    }
}
