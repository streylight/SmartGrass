using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Domains;
using Core.Helpers;
using Map.Repo;
using Service.Interfaces;

namespace Service.Services {
    /// <summary>
    /// The service for the unit class
    /// </summary>
    public class UnitService : IUnitService{

        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<WateringEvent> _wateringEventRepository;
        private readonly IRepository<IrrigationValve> _irrigationValveRepository;
        
        public UnitService() {
            _unitRepository = new Repository<Unit>();    
            _wateringEventRepository = new Repository<WateringEvent>();
            _irrigationValveRepository = new Repository<IrrigationValve>();
        }

        public Unit GetUnitById(int id) {
            return _unitRepository.GetById(id);
        }

        public IList<Unit> GetAllUnits() {
            return _unitRepository.Table.ToList();
        }

        public void Insert(Unit unit) {
            try{
                if (unit.Id == 0) {
                    _unitRepository.Insert(unit);
                    for (var i = 0; i < 24; i++) {
                        _irrigationValveRepository.Insert(new IrrigationValve {
                            ValveNumber = i,
                            UnitId = unit.Id
                        });
                    }
                } else {
                    _unitRepository.Update(unit);
                }

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id) {
            try{
                var unit = _unitRepository.GetById(id);
                _unitRepository.Delete(unit);

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public int ValidateProductKey(string productKey) {
            var unit = _unitRepository.Table.FirstOrDefault(x => x.ProductKey == productKey);
            return unit != null ? unit.Id : -1;
        }

        public Dictionary<string, string> GetValveCommands(int id, Dictionary<int, bool> limitDict, bool temp, bool rain) {
            var irrigationValves = _irrigationValveRepository.Table.Where(x => x.UnitId == id).ToList();
            var commandDict = new Dictionary<string, string>();
            var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);

            foreach (var valve in irrigationValves) {
                var command = "";
                if (valve.WateringEvents.Any(x => x.Watering)) {
                    var wateringEvents = valve.WateringEvents.Where(x => x.Watering).OrderBy(x => x.EndDateTime);
                    var wateringEvent = wateringEvents.First();
                    
                    if (wateringEvent.EndDateTime > now) {
                        if (temp || rain || (limitDict.ContainsKey(valve.ValveNumber) && limitDict[valve.ValveNumber])) {
                            wateringEvent.Watering = false;
                            wateringEvent.IrrigationValve = null;
                            wateringEvent.EndDateTime = now;
                            _wateringEventRepository.Update(wateringEvent);
                            command = "0";
                        } else {
                            command = "1";
                        }
                    } else {
                        wateringEvent.Watering = false;
                        wateringEvent.IrrigationValve = null;
                        _wateringEventRepository.Update(wateringEvent);
                        command = "0";
                    }
                } else if (valve.WateringEvents.Any(x => x.StartDateTime < now && now < x.EndDateTime)) {
                    var wateringEvent = valve.WateringEvents.First(x => x.StartDateTime < now && now < x.EndDateTime);
                    if (temp || rain || (limitDict.ContainsKey(valve.ValveNumber) && limitDict[valve.ValveNumber])) {
                        wateringEvent.Watering = false;
                        wateringEvent.IrrigationValve = null;
                        wateringEvent.EndDateTime = now;
                        command = "0";
                    } else {
                        wateringEvent.Watering = true;
                        wateringEvent.IrrigationValve = null;
                        command = "1";
                    }
                    _wateringEventRepository.Update(wateringEvent);
                } else {
                    command = "0";
                }
                commandDict.Add(valve.ValveNumber.ToString(), command);
            }

            return commandDict;
        }

        public List<object> FilterSoilReadings(FilterType filterBy, int unitId) {
            var unit = _unitRepository.GetById(unitId);
            var data = new List<object>();
            var date = DateTimeHelper.GetLocalTime();
            switch (filterBy) {
                    case FilterType.Week:
                    case FilterType.Month:
                        var soilReadingsByDay = unit.SoilReadings.GroupBy(x => x.DateTime.Date).ToDictionary(x => x.Key, x => x.ToList());
                        var cnt = (int)filterBy;
                        date = date.AddDays((cnt * -1));
                        for (var i = 0; i < cnt; i++) {
                            data.Add(new {
                                d = date.Date.ToString("yyyy-MM-dd"),
                                max = soilReadingsByDay.ContainsKey(date.Date) ? soilReadingsByDay[date.Date].Max(x => x.SoilMoisture) : 0,
                                min = soilReadingsByDay.ContainsKey(date.Date) ? soilReadingsByDay[date.Date].Min(x => x.SoilMoisture) : 0
                            });
                            date = date.AddDays(1);
                        }
                        break;
                    case FilterType.Year:
                        var soilReadingsByMonth = unit.SoilReadings.GroupBy(x => x.DateTime.Month).ToDictionary(x => x.Key, x => x.ToList());
                        for (var i = 1; i <= 12; i++) {
                            data.Add(new {
                                d = new DateTime(date.Year, i, 1),
                                max = soilReadingsByMonth.ContainsKey(i) ? soilReadingsByMonth[i].Max(x => x.SoilMoisture) : 0,
                                min = soilReadingsByMonth.ContainsKey(i) ? soilReadingsByMonth[i].Min(x => x.SoilMoisture) : 0
                            });
                            
                        }
                    break;
            }
            return data;
        }
    }
}
