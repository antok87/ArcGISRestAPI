using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriUK.NETPortalAPI.Parameters
{
    public abstract class RequestGroup : RequestCommon
    {
        public string title {get; set;}
        public string description { get; set; }
        public string snippet { get; set; }
        public string tags { get; set; }
        public string phone { get; set; }
        public string access { get; set; }
        public string isinvitationonly { get; set; }
        public string thumbnail { get; set; }
    }
}
