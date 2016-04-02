

namespace aistream
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AIStreamTelemetryInitializer : ITelemetryInitializer
    {
        public static Action<ITelemetry> Listener { get; set; }

        public void Initialize(ITelemetry telemetry)
        {
            if (Listener != null)
            {
                Listener(telemetry);
            }
        }
    }
}
