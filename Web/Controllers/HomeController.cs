using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core;
using Core.Domains;
using Core.Helpers;
using Service.Interfaces;
using Web.Infrastructure;
using Web.Models;

namespace Web.Controllers {
    public class HomeController : BaseController {

        private readonly IWateringEventService _wateringEventService;
        private readonly IUserService _userService;
        private readonly IIrrigationValveService _irrigationValveService;
        private readonly IUnitService _unitService;

        public HomeController(IWateringEventService wateringEventService, IUserService userService, IIrrigationValveService irrigationValveService, IUnitService unitService) {
            _wateringEventService = wateringEventService;
            _userService = userService;
            _irrigationValveService = irrigationValveService;
            _unitService = unitService;
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test() {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Dashboard() {
            var user = _userService.GetUserById(UserId);
            var wateringEvents = user.Unit.IrrigationValves.SelectMany(x => x.WateringEvents).ToList();
            var tempReadings = user.Unit.TemperatureReadings.ToList();
            var now = DateTimeHelper.GetLocalTime();
            var soilReadings = user.Unit.SoilReadings.OrderByDescending(x => x.DateTime).ToList();

            var model = new DashboardViewModel {
                Unit = user.Unit,
                IrrigationValves = user.Unit.IrrigationValves.ToList(),
                TemperatureByDates = tempReadings.Where(x => x.DateTime > now.AddDays(-7)).GroupBy(x => x.DateTime.Date).ToDictionary(x => x.Key, x => x.Sum(y => y.Temperature) / x.Count()),
                NextScheduledWatering = (wateringEvents.Any(x => x.StartDateTime > now) ? wateringEvents.First(x => x.StartDateTime > now).StartDateTime.ToString("MMM dd hh:mm tt") : "None" ),
                SensorDetails = soilReadings.GroupBy(x => x.SensorNumber).Select(grp => new SensorDetail(grp.Key, grp.First().SoilMoisture, grp.First().DateTime)).ToList()
            };
            return View(model);
        }

        public ActionResult CreateWateringEvent(List<int> irrigationValves, DateTime date, DateTime startTime, DateTime endTime) {
            try {
                var events = new List<EventData>();
                date = date.Date;
                var obj = new {
                    date = date,
                    st = startTime,
                    et = endTime
                };
                foreach (var irrigationValveId in irrigationValves) {
                    var newWateringEvent = new WateringEvent {
                        StartDateTime = date.Add(startTime.TimeOfDay),
                        EndDateTime = date.Add(endTime.TimeOfDay),
                        IrrigationValveId = irrigationValveId
                    };

                    _wateringEventService.Insert(newWateringEvent);

                    events.Add(new EventData(newWateringEvent));
                    //var newObj = new EventData {
                    //    title = startTime.ToString("HH:mm") + "-" + endTime.ToString("HH:mm"),
                    //    start = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssK"),
                    //    end = DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ssK")
                    //};
                    //events.Add(newObj);
                }

                return Json(new { error = false, eventData = events, obj = obj }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(new { error = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult LoadWateringEvent(int id) {
            var user = _userService.GetUserById(UserId);
            var model = new WateringEventViewModel {
                UnitId = user.UnitId,
                WateringEvent = id != 0 ?_wateringEventService.GetWateringEventById(id) : new WateringEvent()
            };
            
            return View("_WateringEventForm", model);
        }

        public ActionResult SetWateringEventAction(int valveId, bool status) {
            var valve = _irrigationValveService.GetIrrigationValveById(valveId);
            var activeWatering = valve.WateringEvents.LastOrDefault(x => x.Watering);
            var user = _userService.GetUserById(UserId);
            var now = DateTimeHelper.GetLocalTime();
            var error = false;

            var message = "";
            if (activeWatering != null && !status) {
                activeWatering.Watering = false;
                activeWatering.EndDateTime = now;
                //_wateringEventService.Insert(activeWatering);
                message = "Watering for valve " + valve.ValveNumber + " has been stopped";
            } else {
                if (valve.WateringEvents.Any(x => x.StartDateTime > now && x.StartDateTime < now.AddMinutes(10))) {
                    message = "Cannot start a watering event when a scheduled watering is about to start";
                    error = true;
                } else {
                    var newWateringEvent = new WateringEvent {
                        IrrigationValveId = valveId,
                        StartDateTime = now,
                        EndDateTime = now.AddMinutes(10),
                        Watering = true
                    };
                    //_wateringEventService.Insert(newWateringEvent);
                    return Json(new {
                        msg = "Watering event started for 10 minutes", jsonEvent = new EventData(newWateringEvent), watering = user.Unit.IrrigationValves.SelectMany(x => x.WateringEvents).Any(x => x.Watering)
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new {
                msg = message, watering = user.Unit.IrrigationValves.SelectMany(x => x.WateringEvents).Any(x => x.Watering), error = error
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GenerateSoilMoistureGraph(FilterType filterBy) {
            var user = _userService.GetUserById(UserId);
            var data = _unitService.FilterSoilReadings(filterBy, user.UnitId);
            return Json(new { results = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetWateringEvents() {
            var user = _userService.GetUserById(UserId);
            var events = user.Unit.IrrigationValves.SelectMany(x => x.WateringEvents).Select(x => new EventData(x)).ToList();
            return Json(new {eventData = events}, JsonRequestBehavior.AllowGet);
        }
    }
}