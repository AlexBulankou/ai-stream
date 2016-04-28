namespace aistream
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using Microsoft.AspNet.SignalR;
    using Microsoft.ApplicationInsights.Channel;
    using System.IO;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }


    public class AIHub : Hub
    {
        private Action<ITelemetry> telemetryListener = null;

        public AIHub()
        {
            AIStreamAggregator.Instance.AreListenersAttached = () => UserHandler.ConnectedIds.Count > 0;
            AIStreamAggregator.Instance.OnItemsReady = this.OnItemsReady;
          
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }


        internal void OnItemsReady(IEnumerable<AIStreamTelemetryItem> items)
        {
            if (UserHandler.ConnectedIds.Count > 0)
            {
                foreach (var connectionId in UserHandler.ConnectedIds)
                {
                    var payload = JsonConvert.SerializeObject(items.ToArray());
                    Clients.Client(connectionId).addItems(payload);
                }
            }
        }

        public void Start(string key)
        {
            if (key == System.Web.Configuration.WebConfigurationManager.AppSettings["aistream:key"])
            {
                UserHandler.ConnectedIds.Add(Context.ConnectionId);
                Clients.Client(Context.ConnectionId).confirmConnected();
            }
        }
    }
}