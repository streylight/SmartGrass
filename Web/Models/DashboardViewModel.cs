using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Domains;

namespace Web.Models {
    public class DashboardViewModel {
        public User User { get; set; }
        public bool Watering { get; set; }
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
            title = wateringEvent.StartDateTime.ToString("HH:mm") + "-" + wateringEvent.EndDateTime.ToString("HH:mm");
            start = wateringEvent.StartDateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
            end = wateringEvent.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
        }

        public string title { get; set; }
        public string start { get;set; }
        public string end { get; set; }
    }
}