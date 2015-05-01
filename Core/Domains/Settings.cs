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
    }
}
