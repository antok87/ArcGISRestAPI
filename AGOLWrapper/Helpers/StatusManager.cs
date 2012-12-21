using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.SharedObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/*
 * This helper class is used for asynchronous Portal operations (e.g. Add Item).
 * When given a job ID, it will poll Portal to check the status of the job,
 * raising an event when the job is complete.
 */

namespace EsriUK.NETPortalAPI.Helpers
{
    public delegate void CheckStatusCompletedEventHandler(object sender, StatusCompletedEventArgs e);

    public class StatusCompletedEventArgs
    {
        public Status response { get; private set; }
        public StatusCompletedEventArgs(Status response)
        {
            this.response = response;
        }
    }

    public class StatusManager
    {
        public event CheckStatusCompletedEventHandler CheckStatusCompletedEvent;
        private PortalConnection portalConn;

        public StatusManager(PortalConnection portalConn)
        {
            this.portalConn = portalConn;
        }

        // call Portal API and poll for updates
        public async Task CheckStatusAsync(string itemId)
        {
            HttpClient httpClient = new HttpClient();
            Status jsonStatusResponse = null;
            string status = null;

            // TODO: Error-handling! What if the job fails?
            // TODO: Remove hardcoding of the polling interval (currently 1000 ms = 1 s)
            while (status != "completed")
            {
                string response = await httpClient.GetStringAsync(string.Format("http://www.arcgis.com/sharing/content/users/{0}/items/{1}/status?f=json&token={2}", portalConn.username, itemId, portalConn.token));

                jsonStatusResponse = JsonConvert.DeserializeObject<Status>(response);
                status = jsonStatusResponse.status;
                await Task.Delay(1000);
            }

            // Raise event to return response
            StatusCompletedEventArgs e = new StatusCompletedEventArgs(jsonStatusResponse);
            CheckStatusCompletedEvent(this, e);
        }        
    }
}
