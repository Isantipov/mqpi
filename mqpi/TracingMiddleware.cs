using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;

namespace mqpi
{
    public class TracingMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly ILog TextLogger =
            LogManager.GetLogger("log4net-default-repository", "TextHttpTracer");

        private static readonly ILog JsonLogger =
            LogManager.GetLogger("log4net-default-repository", "JsonHttpTracer");


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
            string rqBodyCached = ExtractBody(rq);

            var log = GetTextTrace(rq, rqBodyCached);
            TextLogger.Info(log);

            var jsonTrace = GetJsonTrace(rq, rqBodyCached);
            JsonLogger.Info(jsonTrace);
        }

        private static TraceItem GetJsonTrace(HttpRequest rq, string rqBody)
        {
            object bodyAsObject = null;
            try
            {
                bodyAsObject = JsonConvert.DeserializeObject(rqBody);
            }
            catch
            {
                // ignored - do not fail middleware if deserialization failed
            }

            var jsonTrace = new TraceItem
            {
                Date = DateTime.UtcNow,
                Request = new TraceItem.RequestTrace
                {
                    Method = rq.Method,
                    Url = rq.GetDisplayUrl(),
                    Headers = rq.Headers.ToDictionary(i => i.Key, i => (string) i.Value),
                    ParsedBody = bodyAsObject,
                    RawBody = rqBody
                }
            };

            return jsonTrace;
        }

        private static StringBuilder GetTextTrace(HttpRequest rq, string rqBody)
        {
            var log = new StringBuilder();
            log.AppendLine($"{rq.Method} {rq.GetDisplayUrl()}");
            foreach (var header in rq.Headers)
            {
                log.AppendLine($"{header.Key}: {header.Value}");
            }

            if (!string.IsNullOrEmpty(rqBody))
            {
                log.AppendLine(rqBody);
            }

            return log;
        }

        private static string ExtractBody(HttpRequest rq)
        {
            rq.EnableRewind();
            // todo: check what happens if reader is finalized.
            var reader = new StreamReader(rq.Body);
            var content = reader.ReadToEnd();
            rq.Body.Seek(0, SeekOrigin.Begin);
            return content;
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