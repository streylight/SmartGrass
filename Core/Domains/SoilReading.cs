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
        /// The soil sensor number
        /// </summary>
        public int SensorNumber { get; set; }
        /// <summary>
        /// The soil moisture level
        /// </summary>
        public double SoilMoisture { get; set; }
        /// <summary>
        /// The timestamp of the reading
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Foreign key for unit
        /// </summary>
        public int UnitId { get; set; }
        /// <summary>
        /// Virtual property for the unit object
        /// </summary>
        public Unit Unit { get; set; }
    }
}
