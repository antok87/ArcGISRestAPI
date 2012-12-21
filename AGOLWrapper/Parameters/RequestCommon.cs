using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriUK.NETPortalAPI.Parameters
{

    public abstract class RequestCommon : RequestBase
    {
        public string token { get; set; }
        public string format { get; set; }
        public string callback { get; set; }
        public string callback_html { get; set; }

    }
}
