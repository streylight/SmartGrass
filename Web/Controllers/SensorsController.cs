using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core.Domains;
using Service.Interfaces;
using Web.Models;

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
        public string Post([FromBody]SensorDataModel sensorDataModel) {
            //var unitService = new UnitService();
            //var unitId = unitService.ValidateProductKey(sensorDataModel.ProductKey);
            //if (unitId == -1) {
            //    return "Invalid product key.";
            //}

            //var commandDict = unitService.GetValveCommands(unitId);
            //Dictionary<string, object> dict = new Dictionary<string, object>();
            Dictionary<string, object> commands = new Dictionary<string, object>();
            commands.Add("0", "1");
            commands.Add("1", "0");
            commands.Add("2", "0");
            for (var i = 3; i < 24; i++) {
                commands.Add(i.ToString(), "0");
            }

            var serializer = new JavaScriptSerializer();
            var jsonString = serializer.Serialize(commands);
            return jsonString;
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
