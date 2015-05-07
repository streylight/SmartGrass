using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Core.Domains;
using Service.Interfaces;
using Service.Services;
using Web.Models;

namespace Web.Controllers {

    public class SensorsController : ApiController {
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
            try {
                var unitService = new UnitService();
                var soilReadingService = new SoilReadingService();
                var temperatureReadingService = new TemperatureReadingService();
                var rainEventService = new RainEventService();
                var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
                var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);

                var unitId = unitService.ValidateProductKey(sensorDataModel.ProductKey);
                if (unitId == -1) {
                    return "Invalid product key.";
                }
                var unitSettings = unitService.GetUnitById(unitId).Settings;
                var rainFlag = (unitSettings != null && unitSettings.RainLimit < sensorDataModel.Rain);
                //if (unitSettings != null && unitSettings.RainLimit < sensorDataModel.Rain) {
                var rainEvent = new RainEvent {
                    DateTime = now,
                    UnitId = unitId,
                    RainAmount = sensorDataModel.Rain
                };
                rainEventService.Insert(rainEvent);

                //rainFlag = true;
                //}
                var soilLimitsDict = sensorDataModel.SoilReadings.ToDictionary(x => (int.Parse(x.SensorNumber) - 1), x => (unitSettings != null && x.SoilMoisture > unitSettings.SoilMoistureLimit));
                var tempFlag = unitSettings != null && unitSettings.StopOnFreeze && sensorDataModel.Temperature < 34.0;
                var commandDict = unitService.GetValveCommands(unitId, soilLimitsDict, tempFlag, rainFlag);

                soilReadingService.Insert(sensorDataModel.SoilReadings, unitId);
                var tempReading = new TemperatureReading {
                    UnitId = unitId,
                    DateTime = now,
                    Temperature = sensorDataModel.Temperature
                };
                temperatureReadingService.Insert(tempReading);

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
