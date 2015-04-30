using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    public class Settings : BaseEntity {
        public int SoilMoistureLimit { get; set; }
        public int RainLimit { get; set; }
        public bool StopOnFreeze { get; set; }
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
