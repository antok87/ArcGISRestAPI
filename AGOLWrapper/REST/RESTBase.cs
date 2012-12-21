using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsriUK.NETPortalAPI.Parameters;

namespace EsriUK.NETPortalAPI.REST
{
    public abstract class RESTBase
    {
        public Response response { get; set; }
        public Request request { get; set; }
        public PortalConnection portalConn { get; set; }

        public abstract class Response { }
        public abstract class Request { }
        public abstract Object makeRequest();
    }
}
