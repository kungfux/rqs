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
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Name
        {
            get { return "Requirements Search"; }
        }

        public string AcceptedUrlStartsWith
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
            get { return "/api/v1/requirement/"; }
        }

        // TODO: Isolate extensions from accessing web server objects
        public void ProcessRequest(NetworkStream clientStream, Request request)
        {
            Log.Debug("Request for Api is received.");

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
