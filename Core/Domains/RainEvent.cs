using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The rain event class
    /// </summary>
    public class RainEvent : BaseEntity {
        /// <summary>
        /// The timestamp of the event
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// The amount of rain collected
        /// </summary>
        public double RainAmount { get; set; }
        /// <summary>
        /// Foreign key for unit
        /// </summary>
        public int UnitId { get; set; }
        /// <summary>
        /// Virtual property for the unit object
        /// </summary>
        public Unit Unit { get; set; }
    } // class
} // namespace
