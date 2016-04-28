﻿

namespace aistream
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using System;

    public class AIStreamTelemetryInitializer : ITelemetryInitializer
    {
        private static Dedupper<int> dedupper = new Dedupper<int>();

        public static Action<ITelemetry> Listener { get; set; }

        public void Initialize(ITelemetry telemetry)
        {
            AIStreamAggregator.Instance.OnTelemetryTracked(telemetry);
        }
    }
}
