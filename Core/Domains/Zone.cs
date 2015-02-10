using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The zone class
    /// </summary>
    public class Zone : BaseEntity {
        /// <summary>
        /// The name of the zone
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Foreign key for HICS unit
        /// </summary>
        public int UnitId { get; set; }
        /// <summary>
        /// Virtual reference to the HICS_Unit object
        /// </summary>
        public virtual Unit Unit { get; set; }
        /// <summary>
        /// Virtual collection of irrigation valves
        /// </summary>
        public virtual List<IrrigationValve> IrrigationValves { get; set; }
    }
}
