using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The temperature reading class
    /// </summary>
    public class TemperatureReading : BaseEntity {
        /// <summary>
        /// The value of temperature reading
        /// </summary>
        public float Temperature { get; set; }
        /// <summary>
        /// Timestamp of when the readings were taken
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Foreign key for HICS unit
        /// </summary>
        public int UnitId { get; set; }
        /// <summary>
        /// Virtual reference to the HICS_Unit object
        /// </summary>
        public virtual Unit Unit { get; set; }
    }
}
