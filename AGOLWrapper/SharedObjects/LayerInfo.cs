using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsriUK.NETPortalAPI.SharedObjects
{
    // TODO: Describe Layer Info properties below
    public class LayerInfo
    {
        /*
         * "name" : "<layer name>", 
"type" : "Feature Layer",
"description" : "<description>",
"copyrightText" : "<copyright text>",
"geometryType" : "<geometry type>",
"minScale" : <number>,
"maxScale" : <number>,
"extent" : {<extent JSON with spatial ref>},
"drawingInfo" : {<drawing info JSON>},
"hasAttachments" : false,
"htmlPopupType" : "<popup type>",
"objectIdField" : "<object Id field>",
"globalIdField" : "",
"typeIdField" : "<type Id field>",
"fields" : [<array of field JSON objects>],
"types" : [<array of type JSON objects>],
"templates" : [<array of type JSON objects>],
"capabilities" : "<Query and/or Editing, comma separated>"
         * */
        public string name { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public string copyrightText { get; set; }
        public string geometryType { get; set; }
        public int minScale { get; set; }
        public int maxScale { get; set; }
        public Extent extent { get; set; }
        public string drawingInfo { get; set; }
        public bool hasAttachments { get; set; }
        public string htmlPopupType { get; set; }
        public string objectIdField { get; set; }
        public string globalIdField { get; set; }
        public string typeIdField { get; set; }
        public string fields { get; set; }
        public string types { get; set; }
        public string templates { get; set; }
        public string capabilities { get; set; }
    }
}
