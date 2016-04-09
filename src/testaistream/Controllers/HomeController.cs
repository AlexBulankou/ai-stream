using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace testaistream.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Random random = new Random();
            TelemetryClient telemetryClient = new TelemetryClient();
            telemetryClient.TrackTrace("Trace1");

            telemetryClient.TrackEvent(
                "TestEvent1",
                new Dictionary<string, string>() { { "prop1", "val1" } },
                new Dictionary<string, double>() { { "metric1", random.NextDouble() } });

            telemetryClient.TrackEvent(
               "TestEvent1",
               new Dictionary<string, string>() { { "prop1", "val2" } },
               new Dictionary<string, double>() { { "metric1", random.NextDouble() } });

            telemetryClient.TrackTrace("Trace2");

            telemetryClient.TrackException(new InvalidOperationException("Failure"));
            telemetryClient.TrackTrace("Trace3");

            telemetryClient.TrackDependency("TestDependency1", "TestCommand1", DateTimeOffset.Now.AddMilliseconds(-200), TimeSpan.FromMilliseconds(200), true);
            telemetryClient.TrackDependency("TestDependency2", "TestCommand2", DateTimeOffset.Now.AddMilliseconds(-300), TimeSpan.FromMilliseconds(300), false);

            telemetryClient.TrackMetric("RL", System.Net.WebRequest.CreateHttp("http://www.bing.com/search?q=webrequest").GetResponseAsync().Result.GetResponseStream().ReadByte());
            telemetryClient.TrackTrace(new HttpClient().GetStringAsync("http://www.bing.com/search?q=webrequest").Result);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}