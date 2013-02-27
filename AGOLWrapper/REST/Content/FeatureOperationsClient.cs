using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsriUK.NETPortalAPI.REST.Content.UserContent;
using EsriUK.NETPortalAPI.Helpers;
using EsriUK.NETPortalAPI.REST.Content.FeatureOperations;

namespace EsriUK.NETPortalAPI.REST.Content
{
    public class FeatureOperationsClient
    {
        public FeatureOperationsClient(PortalConnection portalConn)
        {
            this.portalConn = portalConn;
        }

        public Analyze.Response Analyze(Analyze.Request request)
        {
            Analyze analyze = new Analyze(portalConn);
            analyze.request = request;
            analyze.makeRequest();
            return analyze.response;
        }

        public void Generate()
        {
            //TODO: Implement Generate()
            throw new NotImplementedException();
        }

        public PortalConnection portalConn { get; set; }
    }
}
