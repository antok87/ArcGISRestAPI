using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriUK.NETPortalAPI.Parameters
{
    public abstract class RequestItem : RequestCommon
    {
        public accessTypes access { get; set; }
        public string title { get; set; }
        public string thumbnail { get; set; }
        public string thumbnailurl { get; set; }
        public string metadata { get; set; }
        public string type { get; set; }
        public string typekeywords { get; set; }
        public string description { get; set; }
        public string tags { get; set; }
        public string snippet { get; set; }
        public string extent { get; set; }
        public string spatialreference { get; set; }
        public string accessinformation { get; set; }
        public string licenseinfo { get; set; }
        public string culture { get; set; }
    }
}
