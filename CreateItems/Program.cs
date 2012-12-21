using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EsriUK.NETPortalAPI;
using EsriUK.NETPortalAPI.Parameters;
using EsriUK.NETPortalAPI.Helpers;
using EsriUK.NETPortalAPI.REST;

/*
 * This demo takes a series of command-line arguments that are URLs to ArcGIS Server
 * services and indexes them on ArcGIS Online
 */ 
namespace CreateItems
{
    class Program
    {
        static void Main(string[] args)
        {
            PortalConnection portalConn = new PortalConnection();

            foreach (string arg in args)
            {
                Console.WriteLine(string.Format("Processing {0}\n", arg));

                string[] names = arg.Split('/');
                string name = string.Format("{0} ({1})", names[names.Length - 2], names[names.Length - 1]);

                //TODO: create proper lookup table to do this
                string type = String.Empty;
                switch (names[names.Length - 1].ToLower())
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
                    default:
                        Console.WriteLine("Ignoring unknown service type " + names[names.Length - 1].ToLower());
                        continue;
                }

                AddItem addItem = new AddItem(portalConn);
                addItem.request.url = arg;
                addItem.request.type = type;
                addItem.request.async = true;
                addItem.request.title = name;
                addItem.request.tags = "index";

                addItem.makeRequest();

                Console.WriteLine("Indexing service with ArcGIS Online\n");


            }
            Console.WriteLine("\nPress Esc to quit");
            while (true)
            {
                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape) break;
            }
        }
    }
}
