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

        private readonly ISoilReadingService _soilReadingService;
        private readonly ITemperatureReadingService _temperatureReadingService;

        public HomeController(IWateringEventService wateringEventService, IUserService userService, IIrrigationValveService irrigationValveService, IUnitService unitService, ISoilReadingService soilReadingService, ITemperatureReadingService temperatureReadingService) {
            _wateringEventService = wateringEventService;
            _userService = userService;
            _irrigationValveService = irrigationValveService;
            _unitService = unitService;
            _soilReadingService = soilReadingService;
            _temperatureReadingService = temperatureReadingService;
        }

        [AllowAnonymous]
        public ActionResult Index() {
            return View();
        }

        //public ActionResult Contact()
        //{
        //    return View();
        //}

        [AppAuthorize(Role.User, Role.Admin)]
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
                NextScheduledWatering = (wateringEvents.Any(x => x.StartDateTime > now) ? wateringEvents.OrderBy(x => x.StartDateTime).First(x => x.StartDateTime > now).StartDateTime.ToString("MMM %d %h:mm tt") : "None" ),
                SensorDetails = soilReadings.GroupBy(x => x.SensorNumber).Select(grp => new SensorDetail(grp.Key, grp.First().SoilMoisture, grp.First().DateTime)).ToList()
            };
            return View(model);
        }

        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult GetUnitStatus() {
            var user = _userService.GetUserById(UserId);
            var now = DateTimeHelper.GetLocalTime();
            return Json(new {active = user.Unit.SoilReadings.Any(x => x.DateTime > now.AddMinutes(-5))}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult DeleteWateringEvent(int id) {
            try {
                _wateringEventService.Delete(id);
                return Json(new {msg = "Watering event successfully removed", error = false});
            } catch (Exception ex) {
                return Json(new {msg = ex.Message, error = true});
            }
        }

        [HttpPost]
        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult CreateWateringEvent(DateTime selectedDate, DateTime startTime, DateTime endTime, List<int> irrigationValveIds) {
            try {
                var events = new List<EventData>();

                foreach (var irrigationValveId in irrigationValveIds) {
                    var valve = _irrigationValveService.GetIrrigationValveById(irrigationValveId);
                    var newWateringEvent = new WateringEvent {
                        StartDateTime = selectedDate.Date.Add(startTime.TimeOfDay),
                        EndDateTime = selectedDate.Date.Add(endTime.TimeOfDay),
                        IrrigationValveId = valve.Id
                    };
                    _wateringEventService.Insert(newWateringEvent);

                    newWateringEvent.IrrigationValve = valve;
                    events.Add(new EventData(newWateringEvent));
                }

                return Json(new { error = false, eventData = events }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(new { error = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult SetWateringEventAction(int valveId, bool status) {
            try {
                var valve = _irrigationValveService.GetIrrigationValveById(valveId);
                var activeWatering = valve.WateringEvents.LastOrDefault(x => x.Watering);
                var user = _userService.GetUserById(UserId);
                var now = DateTimeHelper.GetLocalTime();

                if (activeWatering != null && !status) {
                    activeWatering.IrrigationValve = null;
                    activeWatering.Watering = false;
                    activeWatering.EndDateTime = now;
                    _wateringEventService.Insert(activeWatering);
                    //activeWatering.IrrigationValve = valve;
                    return Json(new {
                        msg = "Watering for valve " + (valve.ValveNumber + 1) + " has been stopped", watering = user.Unit.IrrigationValves.SelectMany(x => x.WateringEvents).Any(x => x.Watering)
                    }, JsonRequestBehavior.AllowGet);
                } 

                if (valve.WateringEvents.Any(x => x.StartDateTime > now && x.StartDateTime < now.AddMinutes(10))) {
                    throw new Exception("Cannot start a watering event when a scheduled watering is about to start");
                }

                var newWateringEvent = new WateringEvent {
                    IrrigationValveId = valveId,
                    StartDateTime = now,
                    EndDateTime = now.AddMinutes(10),
                    Watering = true
                };
                _wateringEventService.Insert(newWateringEvent);
                newWateringEvent.IrrigationValve = valve;
                return Json(new {
                    msg = "Watering event started for 10 minutes", jsonEvent = new EventData(newWateringEvent), watering = user.Unit.IrrigationValves.SelectMany(x => x.WateringEvents).Any(x => x.Watering)
                }, JsonRequestBehavior.AllowGet);

            } catch (Exception ex) {
                return Json(new {msg = ex.Message, error = true});
            }
        }

        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult GenerateSoilMoistureGraph(FilterType filterBy) {
            var user = _userService.GetUserById(UserId);
            var data = _unitService.FilterSoilReadings(filterBy, user.UnitId);
            return Json(new { results = data }, JsonRequestBehavior.AllowGet);
        }

        [AppAuthorize(Role.User, Role.Admin)]
        public ActionResult GetWateringEvents() {
            var user = _userService.GetUserById(UserId);
            var events = user.Unit.IrrigationValves.SelectMany(x => x.WateringEvents).Select(x => new EventData(x)).ToList();
            return Json(new {eventData = events}, JsonRequestBehavior.AllowGet);
        }
    }
}