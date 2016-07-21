using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Web.Script.Serialization;
using log4net;
using Storage.Requirements.Data;
using Storage.Requirements.Model.Entity;
using WebServer.API;
using WebServer.Requests;
using WebServer.Responses;
using WebServer;

namespace RqsWebExtension
{
    public class Extension : IExtension
    {
        //  According to: Web Api Design - Crafting Interfaces that Developers Love

        // /requirement/
        //  Get     List
        //  Post    Create new
        //  Put     Bulk update
        //  Delete  Delete

        // /requirement/FR1
        //  Get     Show FR1
        //  Post    Error
        //  Put     Update if exists
        //  Delete  Delete FR1

        // TODO: We need here: Url, document type?, method?
        // TODO: How to catch both v1 and v2 api requests by the same module?

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Name => "Requirements Search";
        public string AcceptedUrlStartsWith => "/api/v1/requirement/";

        private readonly Requirements _requirementsStorage = new Requirements();

        // TODO: Isolate extensions from accessing web server objects
        public void ProcessRequest(ClientStream clientStream, Request request)
        {
            Log.Debug("Request for Api is received.");

            ICollection<Requirement> reqs = _requirementsStorage.GetRequirements();

            string json = GetJSON(reqs);

            byte[] response = Encoding.UTF8.GetBytes(json);

            clientStream.WriteHeader(new ResponseHeader(System.Net.HttpStatusCode.OK, "application/json", response.Length));
            clientStream.Write(response, 0, response.Length);
        }

        private string GetJSON(ICollection<Requirement> requirements)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(requirements);
            return json;
        }
    }
}
