using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The soil reading class
    /// </summary>
    public class SoilReading : BaseEntity {
        /// <summary>
        /// The soil moisture level
        /// </summary>
        public float MoistureLevel { get; set; }
        /// <summary>
        /// The timestamp of the reading
        /// </summary>
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// Foreign key for irrigation valve
        /// </summary>
        public int IrrigationValveId { get; set; }
        /// <summary>
        /// Virtual property for the irrigation valve object
        /// </summary>
        public IrrigationValve IrrigationValve { get; set; }
    }
}
