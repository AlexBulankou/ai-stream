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

    public static class UserHandler
    {
        public static HashSet<string> ConnectedIds = new HashSet<string>();
    }


    public class AIHub : Hub
    {
        private Action<ITelemetry> telemetryListener = null;

        public AIHub()
        {
            this.telemetryListener = this.OnNewTelemetryItem;
            AIStreamTelemetryInitializer.Listener = this.telemetryListener;
          
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UserHandler.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }


        public void OnNewTelemetryItem(ITelemetry telemetry)
        {
            if (UserHandler.ConnectedIds.Count > 0)
            {
                using (MemoryStream stream = new MemoryStream(JsonSerializer.Serialize(new ITelemetry[] { telemetry }, false)))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string str = reader.ReadToEnd();
                        foreach (var connectionId in UserHandler.ConnectedIds)
                        {
                            Clients.Client(connectionId).addItem(str);
                        }

                    }
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