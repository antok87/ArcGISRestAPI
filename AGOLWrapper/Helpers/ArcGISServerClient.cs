using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using EsriUK.NETPortalAPI;
using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.Helpers;
using EsriUK.NETPortalAPI.SharedObjects;

using Newtonsoft.Json;

namespace EsriUK.NETPortalAPI.Helpers
{
    public class ArcGISServerClient
    {
        public AGSResponse QueryArcGISServer(string server)
        {
            return QueryArcGISServer(server, String.Empty);
        }

        // Request information about services in this folder on an
        // ArcGIS Server instance
        public AGSResponse QueryArcGISServer(string server, string folder)
        {
            if (folder != String.Empty && !folder.EndsWith("/"))
            {
                folder += "/";
            }
            Uri url = new Uri(string.Format("{0}/{1}?f=json", server, folder));
            HttpPostRequest request = new HttpPostRequest(url);

            HttpWebResponse response = request.PostData();
            StreamReader sr = new StreamReader(response.GetResponseStream());
            AGSResponse agsResponse = JsonConvert.DeserializeObject<AGSResponse>(sr.ReadToEnd());

            return agsResponse;
        }
    }
}
