using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.Helpers;
using EsriUK.NETPortalAPI.SharedObjects;

using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace EsriUK.NETPortalAPI.REST.Content.UserContent
{
    public class PublishItem : RESTBase
    {
        public new Request request { get; set; }
        public new Response response { get; private set; }
        public new PortalConnection portalConn { get; set; }

        public PublishItem(PortalConnection portalConn)
        {
            this.request = new Request();
            // Instantiate the publish parameters
            // TODO: Make this more general so that CSV files, etc, can be published
            this.response = null;
            this.portalConn = portalConn;
        }

        public new class Response
        {
            public string type { get; set; }
            public string serviceURL { get; set; }
            public string jobId { get; set; }
            public string serviceItemId { get; set; }
        }

        public new class Request : RequestCommon
        {
            public Request()
            {
                this.publishParameters = new PublishParametersShapefile();
            }
            public string itemId { get; set; }
            public string filetype { get; set; }
            // TODO: Make this more general so that CSV files, etc, can be published
            public PublishParametersShapefile publishParameters { get; set; }
            public string outputType { get; set; }
        }

        public override Object makeRequest()
        {
            //TODO: config URL parts
            Uri url = new Uri(string.Format("http://www.arcgis.com/sharing/content/users/{0}/publish?f=json&token={1}", portalConn.username, portalConn.token));
            HttpPostRequest request = new HttpPostRequest(url);

            request.AddFields(this.request.getParameters());

            HttpWebResponse response = request.PostData();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            Response[] jsonPublishResponse = JsonConvert.DeserializeObject<Response[]>(sr.ReadToEnd());

            //TODO: do this error-checking better
            if (jsonPublishResponse[0].jobId == "")
            {
                throw new Exception();
            }

            return jsonPublishResponse;

        }

   } 
}

