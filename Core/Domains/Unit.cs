using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The HICS unit class
    /// </summary>
    public class Unit : BaseEntity {
        /// <summary>
        /// The product key for the unit
        /// </summary>
        public string ProductKey { get; set; }

        /// <summary>
        /// Virtual property for the irrigation valves
        /// </summary>
        public virtual ICollection<IrrigationValve> IrrigationValves { get; set; }
        /// <summary>
        /// Virtual property for the temperature readings
        /// </summary>
        public virtual ICollection<TemperatureReading> TemperatureReadings { get; set; }
        /// <summary>
        /// Virtual property for the soil readings
        /// </summary>
        public virtual ICollection<SoilReading> SoilReadings { get; set; }
        /// <summary>
        /// Virtual property for the rain events
        /// </summary>
        public virtual ICollection<RainEvent> RainEvents { get; set; }
    }
}
