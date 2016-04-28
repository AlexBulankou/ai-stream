namespace aistream
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;

    class AIStreamEntryTelemetryProcessor:ITelemetryProcessor
    {
        private ITelemetryProcessor nextTelemetryProcessor;

        public AIStreamEntryTelemetryProcessor(ITelemetryProcessor next)
        {
            this.nextTelemetryProcessor = next;
        }

        public void Process(ITelemetry item)
        {
            AIStreamAggregator.Instance.OnTelemetryTracked(item);
            this.nextTelemetryProcessor.Process(item);
        }
    }

    class AIStreamTelemetryProcessor : ITelemetryProcessor
    {
        private ITelemetryProcessor nextTelemetryProcessor;

        public AIStreamTelemetryProcessor(ITelemetryProcessor next)
        {
            this.nextTelemetryProcessor = next;
        }

        public void Process(ITelemetry item)
        {
            AIStreamAggregator.Instance.OnTelemetrySent(item);
            this.nextTelemetryProcessor.Process(item);
        }
    }
}
