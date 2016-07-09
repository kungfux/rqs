using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Web.Script.Serialization;
using log4net;
using RqsWebExtension.Entity;
using WebServer.API;
using WebServer.Requests;
using WebServer.Responses;

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

        // TODO: Isolate extensions from accessing web server objects
        public void ProcessRequest(NetworkStream clientStream, Request request)
        {
            Log.Debug("Request for Api is received.");

            ICollection<Requirement> reqs = new List<Requirement>()
            {
                new Requirement()
                {
                     Id = "FR1",
                     Ccp = "5",
                     Created = DateTime.Now.ToShortDateString(),
                     Modified = DateTime.Now.ToShortDateString(),
                     ObjectNumber = "1.0.0.1",
                     Project = "Pj1",
                     Source = "hardcoded",
                     Status = "New",
                     Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent nec.",
                     TmsTask = "TEST-001"
                },
                new Requirement()
                {
                    Id = "FR2",
                    Ccp = "5",
                     Created = DateTime.Now.ToShortDateString(),
                     Modified = DateTime.Now.ToShortDateString(),
                     ObjectNumber = "1.0.0.2",
                     Project = "Pj1",
                     Source = "hardcoded",
                     Status = "New",
                     Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                     TmsTask = "TEST-002"
                }
            };

            string json = GetJSON(reqs);

            byte[] response = Encoding.UTF8.GetBytes(json);

            Header.Instance.WriteHeader(clientStream, System.Net.HttpStatusCode.OK, "application/json", response.Length);
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
