using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EsriUK.NETPortalAPI.REST.Content.UserContent;
using EsriUK.NETPortalAPI.Helpers;

namespace EsriUK.NETPortalAPI.REST.Content
{
    //public delegate void AddItemCompletedEventHandler(object sender, StatusCompletedEventArgs e);

    public class UserContentClient
    {
        public UserContentClient(PortalConnection portalConn)
        {
            this.portalConn = portalConn;
        }

        public AddItem.Response AddItem(AddItem.Request request)
        {
            AddItem addItem = new AddItem(portalConn);
            addItem.AddItemCompletedEvent += new AddItemCompletedEventHandler(AddItemCompletedEventHandler);
            addItem.request = request;
            addItem.makeRequest();
            return addItem.response;
        }
        private void AddItemCompletedEventHandler(object sender, StatusCompletedEventArgs e)
        {
            // TODO: Error-handling
            if (e.response.status == "completed")
            {
                AddItemCompletedEvent(this, e);
            }
        }
        public event AddItemCompletedEventHandler AddItemCompletedEvent;

        public CreateFolder.Response CreateFolder(string title)
        {
            CreateFolder.Request request = new CreateFolder.Request();
            request.title = title;
            return this.CreateFolder(request);
        }
        public CreateFolder.Response CreateFolder(CreateFolder.Request request)
        {
            CreateFolder cf = new CreateFolder(portalConn);
            cf.request = request;
            cf.makeRequest();
            return cf.response;
        }
        
        public DeleteFolder.Response DeleteFolder(string folderId)
        {
            DeleteFolder.Request request = new DeleteFolder.Request();
            request.id = folderId;
            return this.DeleteFolder(request);
        }
        public DeleteFolder.Response DeleteFolder(DeleteFolder.Request request)
        {
            DeleteFolder df = new DeleteFolder(portalConn);
            df.request = request;
            df.makeRequest();
            return df.response;
        }

        //TODO: This should be in UserContent/UserItem/DeleteItem (ie UserItemClient)
        public DeleteItem.Response DeleteItem(string itemId)
        {
            DeleteItem.Request request = new DeleteItem.Request();
            request.itemId = itemId;
            return this.DeleteItem(request);
        }
        public DeleteItem.Response DeleteItem(DeleteItem.Request request)
        {
            DeleteItem di = new DeleteItem(portalConn);
            di.request = request;
            di.makeRequest();
            return di.response;
        }

        public PublishItem.Response[] PublishItem(PublishItem.Request request, string type)
        {
            PublishItem pi = new PublishItem(portalConn, type);
            pi.request = request;
            return pi.makeRequest() as PublishItem.Response[];
        }

        public PortalConnection portalConn { get; set; }
    }
}
