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
        /// Name identifier for the unit
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Collection of the zones
        /// </summary>
        public virtual ICollection<Zone> Zones { get; set; }
    }
}
