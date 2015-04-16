using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Domains;

namespace Web.Models {
    public class SensorDataModel {
        public List<SoilReading> SoilReadings { get; set; }
        public string ProductKey { get; set; }
        public double Temperature { get; set; }
        public double Rain { get; set; }
    }
}