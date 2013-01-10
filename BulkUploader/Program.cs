using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EsriUK.NETPortalAPI;
using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.Helpers;
using EsriUK.NETPortalAPI.REST.Content.UserContent;
using EsriUK.NETPortalAPI.REST.Content;

/*
 * This demo takes a series of command-line arguments that are paths to compressed
 * shapefiles (.zip files).
 * 
 * Each shapefile is added to ArcGIS Online and published as a feature service
 */ 

namespace BulkUploader
{
    class Program
    {
        static PortalConnection portalConn;

        static void Main(string[] args)
        {
            portalConn = new PortalConnection();

            foreach (string arg in args)
            {
                Console.WriteLine("Uploading shapefile");

                AddItem.Request request = new AddItem.Request();
                request.type = "Shapefile";
                request.file = arg;
                request.filename = arg.Substring(arg.LastIndexOf('\\') + 1);
                request.async = true;
                request.title = request.filename.Substring(0, request.filename.Length - 4);
                request.tags = "bulkupload";

                // Listen for an Add Item completed event
                // When the event is handled, a Publish Item is triggered
                ai.AddItemCompletedEvent += new AddItemCompletedEventHandler(AddItemCompletedEventHandler);

                UserContentClient client = new UserContentClient(portalConn);
                AddItem.Response response = client.AddItem(request);
                if (response.success)
                {
                    Console.WriteLine(string.Format("{0}\nid {1}\nPreparing item for publishing...\n", "Item ", ai.response.id));
                }
                else
                {
                    Console.WriteLine(string.Format("Upload failed {0}\n", response.id));
                }
            }
            //press Esc to quit
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) break;
            }
        }

        // Trigger a Publish Item operation when the Add Item is complete
        private static void AddItemCompletedEventHandler(object sender, StatusCompletedEventArgs e)
        {
            string itemId = e.response.itemId;
            if (e.response.status == "completed")
            {
                Console.WriteLine(string.Format("Publishing Feature Service {0}\n", e.response.itemId));

                PublishItem.Request request = new PublishItem.Request();
                request.publishParameters.name = itemId;
                request.filetype = "shapefile";
                request.token = portalConn.token;
                request.itemId = itemId;
                request.publishParameters.description = "This is a file upload";

                UserContentClient client = new UserContentClient(portalConn);
                PublishItem.Response[] response = client.PublishItem(request);

                foreach (PublishItem.Response serviceResponse in response)
                {
                    Console.WriteLine(string.Format("{0} published", serviceResponse.type));
                    Console.WriteLine(string.Format("{0}", serviceResponse.serviceItemId));
                    Console.WriteLine(string.Format("{0}\n", serviceResponse.serviceURL));
                }

                if (response.Length == 0)
                {
                    Console.WriteLine(string.Format("Service publishing failed\n"));
                }
                // TODO: Since Publish Item is also asynchronous, check whether it has completed
            }
        }
    }
}
