using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriUK.NETPortalAPI.SharedObjects
{
    public class Extent
    {
        public long xmin { get; set; }
        public long xmax { get; set; }
        public long ymin { get; set; }
        public long ymax { get; set; }
        public SpatialReference wkid { get; set; }
    }
}
