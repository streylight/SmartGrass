using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Script.Serialization;
using Core.Domains;
using Core.Helpers;
using Service.Interfaces;
using Service.Services;
using Web.Models;

namespace Web.Controllers {
    /// <summary>
    /// The sensors controller : acts as the sudo API interface for the Raspberry Pis
    /// </summary>
    public class SensorsController : ApiController {
        // Dummy method used for Raspberry Pi testing
        public IEnumerable<string> Get() {
            return new string[] { "value1", "value2" };
        }

        // Dummy method used for Raspberry Pi testing
        public string Get( int id ) {
            return "value";
        }

        /// <summary>
        /// Retrieves values from Raspberry Pi and saves the values
        /// Checks for updated schedule information and returns the values in JSON format to the Pi
        /// </summary>
        /// <param name="sensorDataModel"></param>
        /// <returns></returns>
        public string Post( [FromBody]SensorDataModel sensorDataModel ) {
            try {
                // initialize service instances
                var unitService = new UnitService();
                var soilReadingService = new SoilReadingService();
                var temperatureReadingService = new TemperatureReadingService();
                var rainEventService = new RainEventService();
                var now = DateTimeHelper.GetLocalTime();

                var unitId = unitService.ValidateProductKey( sensorDataModel.ProductKey );
                if ( unitId == -1 ) {
                    return "Invalid product key";
                }
                var unitSettings = unitService.GetUnitById( unitId ).Settings;
                // set rain threshold flag
                var rainFlag = ( unitSettings != null && unitSettings.RainLimit < sensorDataModel.Rain );
                // insert an entry [FOR LOGGING ONLY]
                //if (unitSettings != null && unitSettings.RainLimit < sensorDataModel.Rain) {
                var rainEvent = new RainEvent {
                    DateTime = now,
                    UnitId = unitId,
                    RainAmount = sensorDataModel.Rain
                };
                rainEventService.Insert(rainEvent);

                //rainFlag = true;
                //}
                // set soil moisture threshold flags for each sensor
                var soilLimitsDict = sensorDataModel.SoilReadings.ToDictionary( x => ( int.Parse( x.SensorNumber ) - 1 ), x => ( unitSettings != null && x.SoilMoisture > unitSettings.SoilMoistureLimit ) );
                // set temp threshold flag
                var tempFlag = unitSettings != null && unitSettings.StopOnFreeze && sensorDataModel.Temperature < 34.0;

                // get {0,1} list for each irrigation valve
                var commandDict = unitService.GetValveCommands( unitId, soilLimitsDict, tempFlag, rainFlag );

                soilReadingService.Insert( sensorDataModel.SoilReadings, unitId );
                var tempReading = new TemperatureReading {
                    UnitId = unitId,
                    DateTime = now,
                    Temperature = sensorDataModel.Temperature
                };
                temperatureReadingService.Insert( tempReading );

                var serializer = new JavaScriptSerializer();
                var jsonString = serializer.Serialize( commandDict );
                return jsonString;
            } catch ( Exception ex ) {
                return ex.Message;
            }
        }
    } // class
} // namespace
