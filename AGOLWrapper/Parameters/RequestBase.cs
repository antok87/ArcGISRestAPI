using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Newtonsoft.Json;

using EsriUK.NETPortalAPI.SharedObjects;

namespace EsriUK.NETPortalAPI.Parameters
{
    public abstract class RequestBase
    {
        // first value in list is the default value
        public enum formatTypes { json, pjson, html };
        public enum accessTypes { @private, shared, org, @public };
        public enum userAccessTypes { @private, org, @public };

        // Create a dictionary of key/value pairs from the properties of the request object
        public Dictionary<String, String> getParameters()
        {
            Dictionary<String, String> output = new Dictionary<string, string>();
            foreach (PropertyInfo propInfo in this.GetType().GetProperties())
            {
                object firstValue = propInfo.GetValue(this);

                // A hack to allow a field to be called "callback.html"
                string key = propInfo.Name;
                if (propInfo.Name == "callback_html")
                    key = "callback.html";
                // Rename "format" to "f"
                if (propInfo.Name == "format")
                    key = "f";

                if (firstValue != null)
                {
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    output.Add(key, stripQuotationMarks(JsonConvert.SerializeObject(firstValue, settings)));
                }
            }
            return output;
        }
/*
        private void recurseParameters(string key)
        {
            PropertyInfo[] propInfos = key.GetType().GetProperties();
            if (propInfos.Length == 0)
            {
                return;
            }
            foreach (PropertyInfo propInfo in propInfos)
            {
                object firstValue = propInfo.GetValue(key);
                if (String.IsNullOrEmpty(firstValue.ToString()))
                {

                }
                else
                {
                    recurseParameters(propInfo.Name);
                }
            }
        }
*/
        /*
         * This method removes leading and trailing quotation marks from the input string,
         * e.g. "42" becomes 42
         * 
         * The output type is still a string. If there are no enclosing quotation marks,
         * the string is returned unmodified.
         * 
         * This method is needed because the Portal API does not accept quotation marks around
         * input parameter values
         */
        private string stripQuotationMarks(string input)
        {
            int strLength = input.Length;
            if (strLength < 3) return input;
            string output = input;
            if ((input.Substring(0, 1) == "\"") && (input.Substring(strLength - 1, 1) == "\""))
            {
                output = input.Substring(1, strLength-2);
            }
            return output;
        }
    }
}
