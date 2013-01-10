using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsriUK.NETPortalAPI.Parameters;

namespace EsriUK.NETPortalAPI.REST.Portals.Portal
{
    public class CheckServiceName : RESTBase
    {
        public CheckServiceName(PortalConnection portalConn)
        {
            this.request = new Request();
            this.response = null;
            this.portalConn = portalConn;
        }

        public new Request request { get; set; }
        public new Response response { get; private set; }
        public new PortalConnection portalConn { get; set; }

        public new class Response
        {
            public bool available { get; set; }
        }

        public new class Request : RequestCommon
        {
            public string name { get; set; }
            public string type { get; set; }
        }

        // this API call should be made before an Add Item request
        public override Object makeRequest()
        {
            // TODO: Implement
            // TODO: Need to make a call to "Portal Self" or "User" in order to get the Organization ID
            throw new NotImplementedException();
        }

   } 
}

