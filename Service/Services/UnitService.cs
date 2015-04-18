﻿using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
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

        public Dictionary<string, string> GetValveCommands(int id) {
            var irrigationValves = _irrigationValveRepository.Table.Where(x => x.UnitId == id).ToList();
            var commandDict = new Dictionary<string, string>();
            //var now = DateTime.Now;

            var cstZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cstZone);
            

            foreach (var valve in irrigationValves) {
                var command = "";
                if (valve.WateringEvents.Any(x => x.Watering)) {
                    var wateringEvents = valve.WateringEvents.Where(x => x.Watering).OrderBy(x => x.EndDateTime);
                    var wateringEvent = wateringEvents.First();
                    if (wateringEvent.EndDateTime > now) {
                        command = "1";
                    } else {
                        wateringEvent.Watering = false;
                        wateringEvent.IrrigationValve = null;
                        _wateringEventRepository.Update(wateringEvent);
                        command = "0";
                    }
                } else if (valve.WateringEvents.Any(x => x.StartDateTime < now && now < x.EndDateTime)) {
                    var wateringEvent = valve.WateringEvents.First(x => x.StartDateTime < now && now < x.EndDateTime);
                    wateringEvent.Watering = true;
                    wateringEvent.IrrigationValve = null;
                    _wateringEventRepository.Update(wateringEvent);
                    command = "1";
                } else {
                    command = "0";
                }
                commandDict.Add(valve.ValveNumber.ToString(), command);
            }

            return commandDict;
        }
    }
}
