using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace mqpi
{
    public class  Startup
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
                // Do work that doesn't write to the Response.
                LogRequest(context.Request);
                await next.Invoke();
                // Do logging or other work that doesn't write to the Response.
                //Log.Info($"middleware. Request: {context.Request}");

            });

            app.UseMvc();
            
        }

        private void LogRequest(HttpRequest contextRequest)
        {
            var log = new StringBuilder();
            log.AppendLine($"{contextRequest.Method} {contextRequest.GetDisplayUrl()}");
            foreach (var header in contextRequest.Headers)
            {
                log.AppendLine($"{header.Key}: {header.Value}");
            }

            //var body = 
            
            Log.Info("middleware_" + log);
        }
    }
}
