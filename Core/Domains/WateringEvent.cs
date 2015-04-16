using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The watering event class
    /// </summary>
    public class WateringEvent : BaseEntity {
        /// <summary>
        /// Watering status for irrigation valve
        /// </summary>
        public bool Watering { get; set; }
        /// <summary>
        /// Start date time of the watering
        /// </summary>
        public DateTime StartDateTime { get; set; }
        /// <summary>
        /// End date time of the watering
        /// </summary>
        public DateTime EndDateTime { get; set; }
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
