using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsriUK.NETPortalAPI;
using EsriUK.NETPortalAPI.Helpers;
using EsriUK.NETPortalAPI.REST.Content;
using EsriUK.NETPortalAPI.REST.Content.FeatureOperations;
using EsriUK.NETPortalAPI.REST.Content.UserContent;
using EsriUK.NETPortalAPI.SharedObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSVUploader
{
    class Program
    {
        static PortalConnection portalConn;
        static void Main(string[] args)
        {
            portalConn = new PortalConnection();

            foreach (string arg in args)
            {
                Console.WriteLine("Uploading CSV");

                AddItem.Request request = new AddItem.Request();
                request.type = "CSV";
                request.file = arg;
                request.filename = arg.Substring(arg.LastIndexOf('\\') + 1);
                request.async = true;
                request.title = request.filename.Substring(0, request.filename.Length - 4);
                request.tags = "csvupload";

                // Listen for an Add Item completed event
                // When the event is handled, a Publish Item is triggered
                UserContentClient client = new UserContentClient(portalConn);
                client.AddItemCompletedEvent += new AddItemCompletedEventHandler(AddItemCompletedEventHandler);
                AddItem.Response response = client.AddItem(request);
                if (response.success)
                {
                    Console.WriteLine(string.Format("{0}\nid {1}\nPreparing item for publishing...\n", "Item ", response.id));
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
                Console.WriteLine(string.Format("Analyzing CSV {0}\n", e.response.itemId));
                FeatureOperationsClient foc = new FeatureOperationsClient(portalConn);
                Analyze.Request request = new Analyze.Request();
                request.format = "json";
                request.itemId = e.response.itemId;
                request.type = "csv";
                Analyze.Response response = foc.Analyze(request);

                Console.WriteLine(string.Format("Publishing Feature Service {0}\n", e.response.itemId));

                PublishParametersCSV ppcsv = JsonConvert.DeserializeObject<PublishParametersCSV>(response.publishParameters.ToString());
                //workaround: name returned is invalid on publish!
                ppcsv.name = DateTime.Now.Ticks.ToString();

                PublishItem.Request request2 = new PublishItem.Request("csv");
                request2.publishParameters.name = itemId;
                request2.filetype = "csv";
                request2.token = portalConn.token;
                request2.itemId = itemId;
                request2.publishParameters = ppcsv;

                UserContentClient client = new UserContentClient(portalConn);
                PublishItem.Response[] response2 = client.PublishItem(request2, "csv");

                foreach (PublishItem.Response serviceResponse in response2)
                {
                    Console.WriteLine(string.Format("{0} published", serviceResponse.type));
                    Console.WriteLine(string.Format("{0}", serviceResponse.serviceItemId));
                    Console.WriteLine(string.Format("{0}\n", serviceResponse.serviceURL));
                }

                if (response2.Length == 0)
                {
                    Console.WriteLine(string.Format("Service publishing failed\n"));
                }
                // TODO: Since Publish Item is also asynchronous, check whether it has completed
            }
        }
    }
}
