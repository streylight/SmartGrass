using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domains {
    /// <summary>
    /// The settings class
    /// </summary>
    public class Settings : BaseEntity {
        /// <summary>
        /// The threshold for soil moisture amount 
        /// </summary>
        public int SoilMoistureLimit { get; set; }
        /// <summary>
        /// The threshold for the amount of rain collected
        /// </summary>
        public int RainLimit { get; set; }
        /// <summary>
        /// Trigger to stop watering when freezing conditions are detected
        /// </summary>
        public bool StopOnFreeze { get; set; }
    } // class
} // namespace
