using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using System.IO;
using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.Helpers;
using Newtonsoft.Json;

namespace EsriUK.NETPortalAPI.REST
{
    public class GenerateToken : RESTBase
    {
        public new Request request { get; set; }
        public new Response response { get; private set; }
        public new PortalConnection portalConn { get; set; }

        public GenerateToken(PortalConnection portalConn)
        {
            this.request = new Request();
            this.response = null;
            this.portalConn = portalConn;
        }

        public new class Request : RequestCommon
        {
            public string username { get; set; }
            public string password { get; set; }
            public string expiration { get; set; }
            public string referer { get; set; }
        }

        public new class Response
        {
            public string token { get; set; }
            public string expires { get; set; }
            public bool ssl { get; set; }
        }

        public override Object makeRequest()
        {
            this.request.password = portalConn.password;
            this.request.username = portalConn.username;
            this.request.referer = portalConn.clientReferer;
            // TODO: Don't hardcode this value
            this.request.expiration = "60";
            Uri url = new Uri("https://" + portalConn.portalDomainName + "/sharing/generateToken?f=json");
            HttpPostRequest request = new HttpPostRequest(url);
            request.AddFields(this.request.getParameters());
           
            HttpWebResponse response = request.PostData();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            Response r = JsonConvert.DeserializeObject<Response>(responseString);
            this.response = r;
            return r;
        }
    }
}
