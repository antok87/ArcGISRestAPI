using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EsriUK.NETPortalAPI;
using EsriUK.NETPortalAPI.REST;
using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.Helpers;
using EsriUK.NETPortalAPI.SharedObjects;

/*
 * This demo scans an ArcGIS Server instance and indexes all its contents
 * recursively on ArcGIS Online
 */ 
namespace IndexServerItems
{ 
    class Program
    {
        private static PortalConnection portalConn;
        private static List<string> folderIds = new List<string>();
        private static List<string> itemIds = new List<string>();

        static void Main(string[] args)
        {
            // Connect to Portal/ArcGIS Online
            portalConn = new PortalConnection();
            // Index items on ArcGIS Online
            foreach (string arg in args)
            {
                Console.WriteLine(string.Format("Processing {0}\n", arg));
                QueryArcGISServer(arg, String.Empty, null);
            }
            
            Console.WriteLine("\nPress Enter to delete all services");

            // press Enter to continue
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter) break;
            }

            Console.WriteLine("Deleting...");

            // Delete all the indexed content
            foreach (string folder in folderIds)
            {
                DeleteFolder df = new DeleteFolder(portalConn);
                df.request.id = folder;
                df.makeRequest();
                if (df.response.success)
                {
                    Console.WriteLine(string.Format("Folder {0} {1}", df.response.folder.title, df.response.folder.id));
                }
                else
                {
                    Console.WriteLine(string.Format("Error {0}", df.response.folder.title));
                    Console.WriteLine(string.Format("{0}\n", df.response.folder.id));
                }
            }

            foreach (string item in itemIds)
            {
                DeleteItem di = new DeleteItem(portalConn);
                di.request.itemId = item;
                di.makeRequest();
                if (di.response.success)
                {
                    Console.WriteLine(string.Format("Item {0}", di.response.itemId));
                }
                else
                {
                    Console.WriteLine(string.Format("Error {0}\n", di.response.itemId));
                }
            }

            Console.WriteLine("\nPress Esc to quit");

            // press Esc to quit
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) break;
            }

        }

        private static void QueryArcGISServer(string server, string directory, string parentDirectory)
        {
            ArcGISServerClient AGSClient = new ArcGISServerClient();
            AGSResponse agsResponse = AGSClient.QueryArcGISServer(server, directory);
            foreach (string folder in agsResponse.folders)
            {
                Console.WriteLine(string.Format("====================\n\nCreating folder: {0}", folder));
                CreateFolder cf = new CreateFolder(portalConn);
                cf.request.title = folder;
                cf.makeRequest();
                folderIds.Add(cf.response.folder.id);
                
                // Recursively index all content
                Console.WriteLine(string.Format("Recursing {0} for services\n\n", folder));
                QueryArcGISServer(server, folder, cf.response.folder.id);
            }

            foreach (AGSService service in agsResponse.services)
            {
                Console.WriteLine(string.Format("Indexing service {0}", service.name));

                AddItem ai = new AddItem(portalConn);

                // TODO: create a proper lookup table to do this
                string type = String.Empty;
                switch (service.type.ToLower())
                {
                    case "featureserver":
                        type = "Feature Service";
                        break;
                    case "mapserver":
                        type = "Map Service";
                        break;
                    case "imageserver":
                        type = "Image Service";
                        break;
                    case "gpserver":
                        type = "Geoprocessing Service";
                        break;
                    case "geometryserver":
                        type = "Geometry Service";
                        break;
                    case "geocodeserver":
                        type = "Geocoding Service";
                        break;
                    default:
                        Console.WriteLine("Ignoring unknown service type " + service.type);
                        continue;
                }

                if (!server.EndsWith("/")) server += "/";

                ai.request.url = String.Format("{0}{1}/{2}", server, service.name, service.type);
                ai.request.type = type;
                ai.request.async = true;
                ai.request.title = directory == String.Empty ? service.name : service.name.Substring(directory.Length + 1);
                ai.request.tags = "bulkindex";

                if (parentDirectory == null)
                {
                    ai.makeRequest();
                    itemIds.Add(ai.response.id);
                }
                else
                {
                    ai.request.folderId = parentDirectory;
                    ai.makeRequest();
                }

                Console.WriteLine(string.Format("Completed {0}\n", ai.response.id));
            }
        }

    }
     
}
