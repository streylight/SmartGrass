using System.Collections.Generic;
using Core.Domains;

namespace Web.Models {
    /// <summary>
    /// The SensorDataModel class
    /// </summary>
    public class SensorDataModel {
        public List<SoilReading> SoilReadings { get; set; }
        public string ProductKey { get; set; }
        public double Temperature { get; set; }
        public double Rain { get; set; }
    } // class
} // namespace