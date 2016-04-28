using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Reflection;

[assembly: OwinStartupAttribute("AIStreamStartup", typeof(aistream.AIStreamStartup))]
namespace aistream
{
    public class AIStreamStartup
    {
        private static FieldInfo factoriesFieldInfo = 
            typeof(TelemetryProcessorChainBuilder).GetField("factories", BindingFlags.NonPublic | BindingFlags.Instance);
        
        public void Configuration(IAppBuilder app)
        {
            var builder = TelemetryConfiguration.Active.TelemetryProcessorChainBuilder;
            var factories = (List<Func<ITelemetryProcessor, ITelemetryProcessor>>)factoriesFieldInfo.GetValue(builder);
            factories.Insert(0, (next) => new AIStreamEntryTelemetryProcessor(next));
            factories.Add((next) => new AIStreamTelemetryProcessor(next));
            builder.Build();

            //TelemetryConfiguration.Active.TelemetryInitializers.Add(new AIStreamTelemetryInitializer());
            app.MapSignalR("/aistream", new Microsoft.AspNet.SignalR.HubConfiguration());
        }
    }
}
