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
        public string message { get; set; }
        public int code { get; set; }
        public string[] details { get; set; }
    }
}
