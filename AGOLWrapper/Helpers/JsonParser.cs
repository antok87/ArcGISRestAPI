using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EsriUK.NETPortalAPI.Helpers
{
    // TODO: Use this helper class somewhere!
    public static class JsonParser
    {
        public static string ParseJson(string field, HttpWebResponse response)
        {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            JsonReader jr = new JsonTextReader(reader);
            JObject jObject = JObject.Load(jr);
            
            return jObject.Value<string>(field);
        }
    }
}
