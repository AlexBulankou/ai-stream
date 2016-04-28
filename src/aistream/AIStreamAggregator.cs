namespace aistream
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using System;
    using System.Collections.Generic;
    using System.IO;

    class AIStreamAggregator
    {
        private List<AIStreamTelemetryItem> _items = new List<AIStreamTelemetryItem>();
        private object _itemsLock = new object();
        private static readonly AIStreamAggregator _instance = new AIStreamAggregator();

        public static AIStreamAggregator Instance
        {
            get
            {
                return _instance;
            }
        }

        public void OnTelemetryTracked(ITelemetry telemetryItem)
        {
            if (this.AreListenersAttached != null && this.AreListenersAttached())
            {
                var timeTracked = telemetryItem.Timestamp;
                string id = telemetryItem.GetUniqueID();
                double metricValue = 0;
                if (telemetryItem is RequestTelemetry)
                {
                    metricValue = ((RequestTelemetry)telemetryItem).Duration.TotalMilliseconds;
                }
                else if (telemetryItem is DependencyTelemetry)
                {
                    metricValue = ((DependencyTelemetry)telemetryItem).Duration.TotalMilliseconds;
                }
                else if (telemetryItem is PerformanceCounterTelemetry)
                {
                    metricValue = ((PerformanceCounterTelemetry)telemetryItem).Value;
                }

                else if (telemetryItem is MetricTelemetry)
                {
                    metricValue = ((MetricTelemetry)telemetryItem).Value;
                }

                lock (_itemsLock)
                {
                    _items.Add(
                        new AIStreamTelemetryItem()
                        {
                            TimeAdded = timeTracked,
                            TelemetryItem = telemetryItem,
                            ID = id,
                            MetricValue = metricValue
                        });
                }

            }
        }

        public void OnTelemetrySent(ITelemetry telemetryItem)
        {
            if (this.AreListenersAttached != null && this.AreListenersAttached())
            {
                var timeSent = DateTime.UtcNow;
                var id = telemetryItem.GetUniqueID();

                string telemetryItemID = telemetryItem.Timestamp.ToString();


                List<AIStreamTelemetryItem> itemsToNotify = new List<AIStreamTelemetryItem>();

                lock (_itemsLock)
                {
                    for (int i = 0; i < _items.Count; i++)
                    {
                        if (_items[i].ID == id)
                        {
                            _items[i].TimeSent = timeSent;
                            _items[i].Sent = true;
                            itemsToNotify.Add(_items[i]);
                            _items.RemoveAt(i);
                            break;
                        }
                        else
                        {
                            if (telemetryItem.Timestamp - _items[i].TelemetryItem.Timestamp > TimeSpan.FromSeconds(5))
                            {
                                _items[i].Sent = false;
                                itemsToNotify.Add(_items[i]);
                                _items.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }


                foreach (var itemToNotify in itemsToNotify)
                {
                    if (itemToNotify.TelemetryItem is TraceTelemetry )
                    {
                        if (((TraceTelemetry)itemToNotify.TelemetryItem).Message.Length > 512)
                        {
                            ((TraceTelemetry)itemToNotify.TelemetryItem).Message =
                                ((TraceTelemetry)itemToNotify.TelemetryItem).Message.Substring(0, 512);
                        }
                    }

                    var data = SerializeTelemetryItem(itemToNotify.TelemetryItem);
                    

                    itemToNotify.Data = data;
                }

                if (OnItemsReady != null)
                {
                    OnItemsReady(itemsToNotify);
                }
            }
        }

        public Action<IEnumerable<AIStreamTelemetryItem>> OnItemsReady { get; set; }

        public Func<bool> AreListenersAttached { get; set; }

        private static string SerializeTelemetryItem(ITelemetry telemetryItem)
        {
            using (MemoryStream stream = new MemoryStream(JsonSerializer.Serialize(new ITelemetry[] { telemetryItem }, false)))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    string str = reader.ReadToEnd();
                    return str;
                }
            }
        }
    }
}
