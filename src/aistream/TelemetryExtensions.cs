using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aistream
{
    static class TelemetryExtensions
    {
        public static string GetUniqueID(this ITelemetry telemetryItem)
        {
            if (telemetryItem is RequestTelemetry)
            {
                return "REQUEST-" + ((RequestTelemetry)telemetryItem).Id + "-" + telemetryItem.Timestamp.Ticks.ToString();
            }
            else if (telemetryItem is DependencyTelemetry)
            {
                return "DEPENDENCY-" + ((DependencyTelemetry)telemetryItem).Id + "-" + telemetryItem.Timestamp.Ticks.ToString();
            }
            else if (telemetryItem is EventTelemetry)
            {
                return "EVENT-" + ((EventTelemetry)telemetryItem).Name + "-" + telemetryItem.Timestamp.Ticks.ToString();
            }
            else if (telemetryItem is MetricTelemetry)
            {
                return "METRIC-" + ((MetricTelemetry)telemetryItem).Name + "-" + telemetryItem.Timestamp.Ticks.ToString();
            }
            else if (telemetryItem is PerformanceCounterTelemetry)
            {
                return "PERFCOUNTER-" + ((PerformanceCounterTelemetry)telemetryItem).InstanceName + "-" + telemetryItem.Timestamp.Ticks.ToString();
            }
            else if (telemetryItem is ExceptionTelemetry)
            {
                return "EXCEPTION-" + telemetryItem.Timestamp.Ticks.ToString();
            }
            else if (telemetryItem is PageViewTelemetry)
            {
                return "VIEW-" + ((PageViewTelemetry)telemetryItem).Name + "-" + telemetryItem.Timestamp.Ticks.ToString();
            }
            else if (telemetryItem is TraceTelemetry)
            {
                return "TRACE-" + telemetryItem.Timestamp.Ticks.ToString();
            }
            else if (telemetryItem is SessionStateTelemetry)
            {
                return "SESSIONSTATE-" + telemetryItem.Timestamp.Ticks.ToString();
            }
            else 
            {
                return "TELEMETRY-" + telemetryItem.Timestamp.Ticks.ToString();
            }
        }
    }
}
