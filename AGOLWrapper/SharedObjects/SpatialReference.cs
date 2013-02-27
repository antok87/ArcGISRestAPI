using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriUK.NETPortalAPI.SharedObjects
{
    public class SpatialReference
    {
        public SpatialReference(int wkid)
        {
            this.wkid = wkid;
        }
        public int wkid { get; set; }
    }
}
