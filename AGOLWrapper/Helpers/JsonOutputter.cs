using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace AGOLWrapper.Helpers
{
    class JsonOutputter
    {
        public static string toJson(object thisObject)
        {
            return toJson(thisObject, false);
        }

        public static string toJson(object thisObject, bool indentJson)
        {
            string json;
            if (indentJson)
                json = JsonConvert.SerializeObject(thisObject,
                Formatting.Indented,
                new JsonSerializerSettings { });
            else
                json = JsonConvert.SerializeObject(thisObject,
                Formatting.None,
                new JsonSerializerSettings { });
            return json;
        }

    }

}
