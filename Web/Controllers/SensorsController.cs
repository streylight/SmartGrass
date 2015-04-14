using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace Web.Controllers
{
    public class SensorsController : ApiController
    {
        // GET api/sensors
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/sensors/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/sensors
        public void Post([FromBody]string value)
        {
        }

        // PUT api/sensors/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/sensors/5
        public void Delete(int id)
        {
        }
    }
}
