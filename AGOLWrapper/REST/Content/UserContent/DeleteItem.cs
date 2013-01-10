using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Net;

using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.SharedObjects;
using EsriUK.NETPortalAPI.Helpers;

using Newtonsoft.Json;

namespace EsriUK.NETPortalAPI.REST.Content.UserContent
{
    public class DeleteItem : RESTBase
    {
        public new Request request { get; set; }
        public new Response response { get; private set; }
        public new PortalConnection portalConn { get; set; }

        public DeleteItem(PortalConnection portalConn)
        {
            this.request = new Request();
            this.response = null;
            this.portalConn = portalConn;
        }

        public new class Response
        {
            public bool success { get; set; }
            public string itemId { get; set; }
        }

        public new class Request : RequestCommon
        {
            public string itemId;
        }

        public override Object makeRequest()
        {
            //TODO: config URL parts
            Uri url = new Uri(string.Format("http://www.arcgis.com/sharing/content/users/{0}/items/{1}/delete?f=json&token={2}", portalConn.username, this.request.itemId, portalConn.token));
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

