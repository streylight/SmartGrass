using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Domains;

namespace Web.Models {
    public class DashboardViewModel {
        public User User { get; set; }
        public List<EventData> WateringEvents { get; set; }
    }

    public class EventData {
        public EventData(WateringEvent wateringEvent) {
            title = wateringEvent.StartDateTime.ToString("HH:mm") + "-" + wateringEvent.EndDateTime.ToString("HH:mm");
            start = wateringEvent.StartDateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
            end = wateringEvent.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ssK");
        }

        public string title { get; set; }
        public string start { get;set; }
        public string end { get; set; }
    }
}