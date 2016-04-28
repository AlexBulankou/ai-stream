namespace aistream
{
    using Microsoft.ApplicationInsights.Channel;
    using Newtonsoft.Json;
    using System;

    public class AIStreamTelemetryItem
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonIgnore]
        public ITelemetry TelemetryItem { get; set; }

        [JsonProperty("d")]
        public string Data { get; set; }

        [JsonProperty("s")]
        public bool Sent { get; set; }

        [JsonProperty("ta")]
        public DateTimeOffset TimeAdded { get; set; }

        [JsonProperty("ts")]
        public DateTimeOffset TimeSent { get; set; }

        [JsonProperty("m")]
        public double MetricValue { get; set; }

    }
}
