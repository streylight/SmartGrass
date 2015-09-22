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
        public int ValveNumber { get; set; }
        /// <summary>
        /// Foreign key for unit
        /// </summary>
        public int UnitId { get; set; }

        /// <summary>
        /// Virtual property for the unit object
        /// </summary>
        public Unit Unit { get; set; }
        /// <summary>
        /// Virtual property for the watering events
        /// </summary>
        public virtual ICollection<WateringEvent> WateringEvents { get; set; } 
    } // class
} // namespace
