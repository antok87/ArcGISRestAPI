using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriUK.NETPortalAPI.SharedObjects
{
    // TODO: Describe Publish Parameters for CSV files below
    public class PublishParametersCSV : PublishParameters
    {
        public PublishParametersCSV()
        {
            this.sourceSR = new SpatialReference(4326);
            this.columnDelimiter = ",";
            /* nulls can be ignored with JsonSerializerSettings
            this.addressTemplate = String.Empty;
            this.lookupType = String.Empty;
            this.lookupFields = String.Empty;
            this.columnNames = String.Empty;
             */

            //TODO: set default values
        }

        public string locationType { get; set; }
        public string latitudeFieldName { get; set; }
        public string longitudeFieldName { get; set; }
        public string addressTemplate { get; set; }
        public string lookupType { get; set; }
        public string lookupFields { get; set; }
        public string columnNames { get; set; }
        public string columnDelimiter { get; set; }
        public SpatialReference sourceSR { get; set; }

        /*
        public string message { get; set; }
        public int code { get; set; }
        public string[] details { get; set; }
         */ //MW: not sure what these are Simon?!
    }
}
