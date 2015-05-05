using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Domains;

namespace Web.Models {
    public class WateringEventViewModel {
        public WateringEvent WateringEvent { get; set; }
        public int UnitId { get; set; }
    }
}