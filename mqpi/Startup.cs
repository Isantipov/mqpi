using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace mqpi
{
    public class Startup
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(Startup));

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.Use(async (context, next) =>
            {
                LogRequest(context.Request);
                await next.Invoke();
            });

            app.UseMvc();
        }

        private static void LogRequest(HttpRequest rq)
        {
            var log = new StringBuilder();
            log.AppendLine($"{rq.Method} {rq.GetDisplayUrl()}");
            foreach (var header in rq.Headers)
            {
                log.AppendLine($"{header.Key}: {header.Value}");
            }

            rq.EnableRewind();
            // todo: check what happens if reader is finalized.
            var reader = new StreamReader(rq.Body);
            var content = reader.ReadToEnd();
            rq.Body.Seek(0, SeekOrigin.Begin);
            if (!string.IsNullOrEmpty(content))
            {
                log.AppendLine(content);
            }

            Log.Info(log);
        }
    }
}
