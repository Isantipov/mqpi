using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace mqpi.Controllers
{
    [Route("api/{type}")]
    public class MockedController : Controller
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(MockedController));

        private readonly Random _randomId = new Random(DateTime.UtcNow.Second);

        // GET api/values
        [HttpGet]
        public IEnumerable<dynamic> Get(string type)
        {
            Log.Info($"GET {type}");
            return new dynamic[]{};
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult Get(string type, int id)
        {
            Log.Info($"GET {type} id = {id}");
            return new NotFoundObjectResult($"{type} with id='{id}' was not found");
        }

        // POST api/values
        [HttpPost]
        public object Post(string type, [FromBody]dynamic value)
        {
            value.Id = _randomId.Next();

            return value;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public dynamic Put(string type, [FromBody]dynamic value, int id)
        {
            if (value.Id != id)
            {
                return new BadRequestObjectResult("Id in the object must match id in URI");
            }

            return value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public ActionResult Delete(string type, int id)
        {
            return Ok($"{type} {id} has been removed");
        }
    }
}
