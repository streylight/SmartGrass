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
    }
}