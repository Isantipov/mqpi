using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using IOFile = System.IO.File;

namespace mqpi.Controllers
{
    [Route("_sys/Traces")]
    public class TracesController : Controller
    {
        public string Get(int offset = 0, int limit = 100)
        {
            
            var lines = ReadLines()
                .Skip(offset).Take(limit);
            var result = string.Join(Environment.NewLine, lines);
            
            return result;
        }

        private static IEnumerable<string> ReadLines()
        {
            using (FileStream file = IOFile.Open("../Logs/mqpi4.log", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
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