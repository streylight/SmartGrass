using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Core.Domains;

namespace Web.Models {
    public class DashboardViewModel {
        public Unit Unit { get; set; }
        public List<IrrigationValve> IrrigationValves { get; set; }
        public Dictionary<DateTime, double> TemperatureByDates { get; set; }
        public string SoilMoisture { get; set; }
        public string NextScheduledWatering { get; set; }
        public List<SensorDetail> SensorDetails { get; set; } 
    }

    public class SensorDetail {
        public SensorDetail(string sensorNumber, double moisture, DateTime collectedDateTime) {
            SensorNumber = sensorNumber.Trim();
            MoistureLevel = moisture*20; // multiple by 20 to covert 0-5 range to 0-100
            LastUpdated = collectedDateTime;
        }
        public double MoistureLevel { get; set; }
        public string SensorNumber { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class EventData {
        public EventData(WateringEvent wateringEvent) {
            var fi = new DateTimeFormatInfo {
                AMDesignator = "a",
                PMDesignator = "p"
            };
            title = wateringEvent.StartDateTime.ToString("%h:mmtt", fi) + "-" + wateringEvent.EndDateTime.ToString("%h:mmtt", fi);
            start = wateringEvent.StartDateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
            end = wateringEvent.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
            id = wateringEvent.Id;
            valve = string.Format("{0}", wateringEvent.IrrigationValve.ValveNumber + 1);
        }

        public int id { get; set; }
        public string title { get; set; }
        public string start { get;set; }
        public string end { get; set; }
        public string valve { get; set; }
    }
}