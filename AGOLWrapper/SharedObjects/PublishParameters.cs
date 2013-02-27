using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EsriUK.NETPortalAPI.Parameters;

namespace EsriUK.NETPortalAPI.SharedObjects
{
    public abstract class PublishParameters : RequestBase
    {
        public PublishParameters()
        {
            this.maxRecordCount = -1;
            this.targetSR = new SpatialReference(102100);
            this.layerInfo = new Object();
            /* nulls can be ignored in Json SerializerSettings
            this.copyrightText = String.Empty;
            this.description = String.Empty;
             */
        }
        public string name { get; set; }
        public string copyrightText { get; set; }
        public string description { get; set; }
        public object layerInfo { get; set; }
        public int maxRecordCount { get; set; }
        public SpatialReference targetSR { get; set; }
    }
}