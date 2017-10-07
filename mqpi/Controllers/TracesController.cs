using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using IOFile = System.IO.File;

namespace mqpi.Controllers
{
    [Route("_sys/traces")]
    public class TracesController : Controller
    {
        public string Get(int offset = 0, int limit = 100)
        {
            var lines = ReadLines()
                .Skip(offset).Take(limit);
            var result = string.Join(Environment.NewLine, lines);

            return result;
        }

        [Route("json")]
        public ActionResult GetJson(int offset = 0, int limit = 100)
        {
            IEnumerable<TraceItem> traceItems = OpenJsonFile().Skip(offset).Take(limit);
            return Ok(traceItems);
        }

        private IEnumerable<TraceItem> OpenJsonFile()
        {
            using (FileStream file = IOFile.Open("/var/log/mqpi/json_traces.log", FileMode.OpenOrCreate,
                FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader streamReader = new StreamReader(file))
            using (JsonTextReader reader = new JsonTextReader(streamReader))
            {
                reader.SupportMultipleContent = true;


                var serializer = new JsonSerializer();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader.LineNumber}:{reader.LinePosition}");

                    if (reader.TokenType == JsonToken.StartObject)
                    {
                        var c = serializer.Deserialize<TraceItem>(reader);
                        yield return c;
                    }
                }
            }
        }

        private static IEnumerable<string> ReadLines()
        {
            using (FileStream file = IOFile.Open("/var/log/mqpi/text.log", FileMode.OpenOrCreate, FileAccess.Read,
                FileShare.ReadWrite))
            {
                using (var textReader = new StreamReader(file))
                {
                    while (!textReader.EndOfStream)
                    {
                        yield return textReader.ReadLine();
                    }
                }
            }
        }
    }
}