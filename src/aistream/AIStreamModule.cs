using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace aistream
{
    public class AIStreamModule : IHttpModule
    {

        public void Dispose()
        {

        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Path.EndsWith(System.Web.Configuration.WebConfigurationManager.AppSettings["aistream:path"], StringComparison.OrdinalIgnoreCase))
            {
                using (var stream = typeof(AIStreamModule).Assembly.GetManifestResourceStream("aistream.aistream.html"))
                {
                    using (var streamReader = new StreamReader(stream))
                    {
                        HttpContext.Current.Response.Write(streamReader.ReadToEnd());
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.Close();
                    }
                }
            }
        }
    }
}
