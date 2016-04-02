using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute("AIStreamStartup", typeof(aistream.AIStreamStartup))]
namespace aistream
{
    public class AIStreamStartup
    {
        public void Configuration(IAppBuilder app)
        {
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new AIStreamTelemetryInitializer());
            app.MapSignalR("/aistream", new Microsoft.AspNet.SignalR.HubConfiguration());
        }
    }
}
