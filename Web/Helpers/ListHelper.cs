using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Domains;
using Service.Interfaces;

namespace Web.Helpers {
    public static class ListHelper {

        public static List<IrrigationValve> GetListOfIrrigationValves(int unitId) {
            var irrigationValveService = new IrrigationValveService();
            return irrigationValveService.GetAllIrrigationValves().Take(3).ToList();
        }

        public static List<DateTime> GetPreviousWeekDates(DateTime now) {
            var startingDate = now.AddDays(-6);
            var datesList = new List<DateTime> {startingDate};
            for (var i = 0; i < 6; i++) {
                datesList.Add(startingDate.AddDays(i));
            }
            return datesList;
        } 
    }
}