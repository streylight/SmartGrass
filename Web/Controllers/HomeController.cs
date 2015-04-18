using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Domains;
using Service.Interfaces;
using Web.Infrastructure;
using Web.Models;

namespace Web.Controllers {
    public class HomeController : BaseController {

        private readonly IWateringEventService _wateringEventService;
        private readonly IUserService _userService;

        public HomeController(IWateringEventService wateringEventService, IUserService userService) {
            _wateringEventService = wateringEventService;
            _userService = userService;
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
            var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
            var model = new DashboardViewModel {
                User = user,
                Watering = wateringEvents.Any(x => x.Watering),
                CurrentTemperature = tempReadings.Any() ? string.Format("{0}°F", tempReadings.OrderBy(x => x.DateTime).First().Temperature + 33.8) : "N/A",
                NextScheduledWatering = (wateringEvents.Any(x => x.StartDateTime > now) ? wateringEvents.First(x => x.StartDateTime > now).StartDateTime.ToString() : "None" )
            };
            return View(model);
        }

        public ActionResult CreateWateringEvent(List<int> irrigationValves, DateTime date, DateTime startTime, DateTime endTime) {
            try {
                var events = new List<EventData>();
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

                return Json(new { error = false, eventData = events }, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(new { error = true, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetWateringEvents() {
            var user = _userService.GetUserById(UserId);
            var events = user.Unit.IrrigationValves.SelectMany(x => x.WateringEvents).Select(x => new EventData(x)).ToList();
            return Json(new {eventData = events}, JsonRequestBehavior.AllowGet);
        }
    }
}