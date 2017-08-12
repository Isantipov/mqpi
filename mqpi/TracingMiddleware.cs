using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;

namespace mqpi
{
    public class TracingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(TracingMiddleware));


        public TracingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            LogRequest(context.Request);

            // Call the next delegate/middleware in the pipeline
            return _next(context);
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

    public static class TracingMiddlewareExtensions
    {
        public static IApplicationBuilder UseTracing(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TracingMiddleware>();
        }
    }
}