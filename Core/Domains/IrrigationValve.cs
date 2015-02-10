using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The irrigation valve class
    /// </summary>
    public class IrrigationValve : BaseEntity {
        /// <summary>
        /// Unique identifier used to distinguish valves
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// Foreign key for zone
        /// </summary>
        public int ZoneId { get; set; }
        /// <summary>
        /// Virtual property for the zone object
        /// </summary>
        public virtual Zone Zone { get; set; }
        /// <summary>
        /// Collection of soil readings for the valve
        /// </summary>
        public virtual ICollection<SoilReading> SoilReadings { get; set; }
    }
}
