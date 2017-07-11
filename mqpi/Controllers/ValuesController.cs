using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Rest;

namespace mqpi.Controllers
{
    [Route("api/{type}")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get(string type)
        {
            return new string[]{"smth", "smth2"};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(string type, int id)
        {
            return new NotFoundObjectResult($"{type} with id='{id}' was not found");
        }

        // POST api/values
        [HttpPost]
        public object Post(string type, [FromBody]dynamic value)
        {
            var random = new Random(DateTime.UtcNow.Second);
            value.Id = random.Next();

            return value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public dynamic Put(string type, [FromBody]dynamic value, int id)
        {
            if (value.id != id)
            {
                return new BadRequestObjectResult("Id in the object must match id in URI");
            }

            return value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
