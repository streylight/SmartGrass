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
        public double Temperature { get; set; }
        /// <summary>
        /// DateTime of when the readings were taken
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// Foreign key for unit
        /// </summary>
        public int UnitId { get; set; }
        /// <summary>
        /// Virtual reference to the unit object
        /// </summary>
        public virtual Unit Unit { get; set; }
    } // class
} // namespace