using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriUK.NETPortalAPI.SharedObjects
{
    // TODO: Describe Publish Parameters for shapefiles below
    public class PublishParametersShapefile : PublishParameters
    {
        public string name { get; set; }
        public string description { get; set; }
        /*
        public string maxRecordCount { get; set; }
        public string copyrightText { get; set; }
        public string layerInfo { get; set; }
        public SpatialReference targetSR { get; set; }*/
    }
}
