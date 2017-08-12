using System;
using Newtonsoft.Json;

namespace mqpi
{
    public class TraceItem
    {
        public class RequestTrace
        {
            public string Method { get; set; }
            public string Url { get; set; }
        }

        public DateTime Date { get; set ;}

        public RequestTrace Request { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}