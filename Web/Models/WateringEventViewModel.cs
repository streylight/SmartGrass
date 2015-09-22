using System;
using Core.Domains;

namespace Web.Models {
    /// <summary>
    /// The WateringEventViewModel class
    /// </summary>
    public class WateringEventViewModel {
        public WateringEvent WateringEvent { get; set; }
        public DateTime SelectedDate { get; set; }
        public int UnitId { get; set; }
    } // class
} // namespace