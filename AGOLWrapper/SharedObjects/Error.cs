using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriUK.NETPortalAPI.SharedObjects
{
    public class Error
    {
        public string message { get; set; }
        public int code { get; set; }
        public string[] details { get; set; }
    }
}
