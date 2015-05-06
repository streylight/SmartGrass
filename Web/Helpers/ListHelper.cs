using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Core;
using Core.Domains;
using Service.Interfaces;

namespace Web.Helpers {
    public static class ListHelper {

        public static List<IrrigationValve> GetListOfIrrigationValves(int unitId) {
            var irrigationValveService = new IrrigationValveService();
            return irrigationValveService.GetAllIrrigationValves().Take(3).ToList();
        }

        public static List<DateTime> GetPreviousWeekDates(DateTime now) {
            var startingDate = now.AddDays(-7);
            var datesList = new List<DateTime> {startingDate};
            for (var i = 1; i < 7; i++) {
                datesList.Add(startingDate.AddDays(i));
            }
            return datesList;
        }

        public static Dictionary<Role, string> GetListRoles() {
            return Enum.GetValues(typeof(Role)).Cast<object>().ToDictionary(at => (Role)at, at => GetEnumDescription((Role)at));
        }

        private static string GetEnumDescription(Enum value) {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            
            return value.ToString();
        }
    }
}