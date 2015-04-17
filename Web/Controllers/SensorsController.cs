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
            var unitService = new UnitService();
            var soilReadingService = new SoilReadingService();
            var temperatureReadingService = new TemperatureReadingService();
            try {
                var unitId = unitService.ValidateProductKey(sensorDataModel.ProductKey);
                if (unitId == -1) {
                    return "Invalid product key.";
                }

                soilReadingService.Insert(sensorDataModel.SoilReadings, unitId);
                var tempReading = new TemperatureReading {
                    UnitId = unitId,
                    DateTime = DateTime.Now,
                    Temperature = sensorDataModel.Temperature
                };
                temperatureReadingService.Insert(tempReading);

                var commandDict = unitService.GetValveCommands(unitId);

                var serializer = new JavaScriptSerializer();
                var jsonString = serializer.Serialize(commandDict);
                return jsonString;
            } catch (Exception ex) {
                return ex.Message;
            }
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
