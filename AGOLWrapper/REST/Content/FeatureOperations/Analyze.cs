using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EsriUK.NETPortalAPI.Helpers;
using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.SharedObjects;
using Newtonsoft.Json;

namespace EsriUK.NETPortalAPI.REST.Content.FeatureOperations
{
    public class Analyze : RESTBase
    {
        public new Request request { get; set; }
        public new Response response { get; private set; }
        public new PortalConnection portalConn { get; set; }

        public new class Response
        {
            public long fileSize;
            public string fileUrl;
            public object publishParameters; //public PublishParameters publishParameters;
            public List<object> records;
        }

        public new class Request : RequestCommon
        {
            public string itemId { get; set;}
            public string file {get;set;}
            public string text {get; set;}
            //TODO: enum? - shapefile | csv
            public string type {get; set;}
        }

        public Analyze(PortalConnection portalConn)
        {
            this.request = new Request();
            this.portalConn = portalConn;
            this.request.token = portalConn.token;
        }


        public override Object makeRequest()
        {
            string protocol = portalConn.alwaysUseSSL ? "https://" : "http://";
            Uri url = new Uri(String.Format("{0}{1}/sharing/rest/content/features/analyze?token={2}", protocol, portalConn.portalDomainName, portalConn.token));
            HttpPostRequest request = new HttpPostRequest(url);
            request.AddFields(this.request.getParameters());
            HttpWebResponse response = request.PostData();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            this.response = JsonConvert.DeserializeObject<Response>(responseString);
            if (this.response == null)
            {
                // TODO: Error Handling
            }
            return this.response;
        }
    }
}
