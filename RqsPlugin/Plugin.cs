using FuseWebServer.API;
using FuseWebServer.Requests;
using FuseWebServer.Responses;
using log4net;
using System;
using System.Net.Sockets;
using System.Text;
using RqsPlugin.Entity;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace RqsPlugin
{
    public class Plugin : IPlugin
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

            ICollection<Requirement> reqs = new List<Requirement>()
            {
                new Requirement()
                {
                     ID = "FR1"
                },
                new Requirement()
                {
                    ID = "FR2"
                }
            };

            string json = GetJSON(reqs);

            byte[] response = Encoding.UTF8.GetBytes(json);

            Header.Instance.WriteHeader(clientStream, System.Net.HttpStatusCode.OK, "application/json", response.Length);
            clientStream.Write(response, 0, response.Length);
        }

        private byte[] GetBytes(string value)
        {
            byte[] bytes = new byte[value.Length * sizeof(char)];
            Buffer.BlockCopy(value.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string GetJSON(ICollection<Requirement> requirements)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string json = serializer.Serialize(requirements);
            return json;
        }
    }
}
