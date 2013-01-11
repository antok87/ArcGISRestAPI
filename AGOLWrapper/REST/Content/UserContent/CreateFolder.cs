using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.SharedObjects;
using EsriUK.NETPortalAPI.Helpers;
using System.Net;
using System.IO;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EsriUK.NETPortalAPI.REST.Content.UserContent
{
    public class CreateFolder : RESTBase
    {
        public new Request request { get; set; }
        public new Response response { get; private set; }
        public new PortalConnection portalConn { get; set; }

        public new class Response
        {
            public bool success { get; set; }
            public Folder folder { get; set; }
        }

        public new class Request : RequestCommon
        {
            public string title { get; set; } 
        }

        public CreateFolder(PortalConnection portalConn)
        {
            this.request = new Request();
            this.portalConn = portalConn;
            this.request.token = portalConn.token;
        }


        public override Object makeRequest()
        {
            //TODO: config URL parts (http)
            Uri url = new Uri("http://" + portalConn.portalDomainName + "/sharing/rest/content/users/" + portalConn.username + "/createFolder?f=json&token=" + portalConn.token);
            HttpPostRequest request = new HttpPostRequest(url);
            request.AddFields(this.request.getParameters());
            HttpWebResponse response = request.PostData();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            this.response = JsonConvert.DeserializeObject<Response>(responseString);
            if (!this.response.success)
            {
                // TODO: Error Handling
            }
            return this.response;
        }
    }
}
