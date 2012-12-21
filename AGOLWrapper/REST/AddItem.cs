using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.SharedObjects;
using EsriUK.NETPortalAPI.Helpers;

using Newtonsoft.Json;

using System.IO;
using System.Net;

namespace EsriUK.NETPortalAPI.REST
{
    public delegate void AddItemCompletedEventHandler(object sender, StatusCompletedEventArgs e);

    public class AddItem : RESTBase
    {
        public event AddItemCompletedEventHandler AddItemCompletedEvent;
        public new PortalConnection portalConn { get; set; }

        public new Request request { get; set; }
        public new Response response { get; private set; }

        public AddItem(PortalConnection portalConn)
        {
            this.request = new Request();
            this.response = null;
            this.portalConn = portalConn;
        }

        // Trigger an AddItemCompletedEvent when the Status Manager raises a
        // CheckStatusCompletedEvent
        private void CheckStatusCompletedEventHandler(object sender, StatusCompletedEventArgs e)
        {
            // TODO: Error-handling
            if (e.response.status == "completed")
            {
                AddItemCompletedEvent(this, e);
            }
        }

        public new class Response
        {
            public bool success { get; set; }
            public string id { get; set; }
            public string folder { get; set; }
        }

        public new class Request : RequestItem
        {
            public string file { get; set; }
            public string url { get; set; }
            public string text { get; set; }
            public string relationshipType { get; set; }
            public string originItemId { get; set; }
            public string destinationItemId { get; set; }
            public bool async { get; set; }
            public bool multipart { get; set; }
            public string filename { get; set; }
            public string folderId { get; set; }
        }

        public override Object makeRequest()
        {
            if (this.request.async)
            {
                if (this.request.folderId != String.Empty)
                {
                    this.request.folderId += "/";
                }

                Uri url = new Uri(string.Format("http://www.arcgis.com/sharing/content/users/{0}/{1}addItem?f=json&token={2}", portalConn.username, this.request.folderId, portalConn.token));
                HttpPostRequest request = new HttpPostRequest(url);

                if ((this.request.file != String.Empty) && (this.request.file != null))
                {
                    request.AddFile("file", this.request.file, this.request.filename);
                }
                Dictionary<String, String> parameters = this.request.getParameters();
                // Remove "file" request parameter otherwise it conflicts with the "file"
                // added above
                parameters.Remove("file");

                request.AddFields(parameters);

                // Submit an Add Item request
                HttpWebResponse response = request.PostData();
                StreamReader sr = new StreamReader(response.GetResponseStream());
                Response jsonAddItemResponse = JsonConvert.DeserializeObject<Response>(sr.ReadToEnd());
                this.response = jsonAddItemResponse;

                // TODO: do this error-checking better
                if (!jsonAddItemResponse.success)
                {
                    throw new Exception();
                }

                // If Add Item submission was successful, start polling the Portal API
                // to check when the asynchronous Add Item operation is complete
                if ((this.request.file != String.Empty) && (this.request.file != null))
                {
                    // Create a Status Manager so that an event is triggered when the asynchronous
                    // Add Item operation is complete. The user can then use this event to start
                    // a Publish Item operation if necessary
                    StatusManager statusManager = new StatusManager(this.portalConn);
                    statusManager.CheckStatusCompletedEvent += new CheckStatusCompletedEventHandler(CheckStatusCompletedEventHandler);
                    Task t = statusManager.CheckStatusAsync(jsonAddItemResponse.id);
                }
            }
            else // TODO: Implement synchronous Add Item
            {
                throw new NotImplementedException();
            }

            return null;
        }
   } 
}

