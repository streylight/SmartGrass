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
    public class UnitService : IUnitService {

        #region vars

        private readonly IRepository<Unit> _unitRepository;
        private readonly IRepository<WateringEvent> _wateringEventRepository;
        private readonly IRepository<IrrigationValve> _irrigationValveRepository;
        private const int MAX_VALVE_CNT = 24;
        #endregion

        #region ctor

        /// <summary>
        /// Constructor for the UnitService class
        /// </summary>
        public UnitService() {
            _unitRepository = new Repository<Unit>();    
            _wateringEventRepository = new Repository<WateringEvent>();
            _irrigationValveRepository = new Repository<IrrigationValve>();
        }

        #endregion

        #region methods

        /// <summary>
        /// Returns a Unit with matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Unit GetUnitById( int id ) {
            return _unitRepository.GetById( id );
        }

        /// <summary>
        /// Returns all Units
        /// </summary>
        /// <returns></returns>
        public IList<Unit> GetAllUnits() {
            return _unitRepository.Table.ToList();
        }

        /// <summary>
        /// Inserts a new Unit into the db and creates its irrigation valve objects
        /// </summary>
        /// <param name="unit"></param>
        public void Insert( Unit unit ) {
            if ( unit.Id == 0 ) {
                _unitRepository.Insert( unit );
                for ( var i = 0; i < MAX_VALVE_CNT; i++ ) {
                    _irrigationValveRepository.Insert( new IrrigationValve {
                        ValveNumber = i,
                        UnitId = unit.Id
                    } );
                }
            } else {
                _unitRepository.Update( unit );
            }

        }

        /// <summary>
        /// Delete a Unit form the db [ADMIN ONLY]
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id) {
            var unit = _unitRepository.GetById( id );
            if ( unit == null ) {
                throw new Exception( string.Format( "No unit found with id: {0}", id ) );
            }
            _unitRepository.Delete( unit );
        }

        /// <summary>
        /// Validates if a product key is valid
        /// </summary>
        /// <param name="productKey"></param>
        /// <returns></returns>
        public int ValidateProductKey( string productKey ) {
            var unit = _unitRepository.Table.FirstOrDefault( x => x.ProductKey == productKey );
            return unit != null ? unit.Id : -1;
        }

        /// <summary>
        /// Gets all on/off events for irrigation valves based on schedule and settings thresholds
        /// Command 0 = turn valve off
        /// Command 1 = turn valve on
        /// </summary>
        /// <param name="id"></param>
        /// <param name="limitDict"></param>
        /// <param name="temp"></param>
        /// <param name="rain"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetValveCommands( int id, Dictionary<int, bool> limitDict, bool temp, bool rain ) {
            var irrigationValves = _irrigationValveRepository.Table.Where( x => x.UnitId == id ).ToList();
            var commandDict = new Dictionary<string, string>();
            var now = DateTimeHelper.GetLocalTime();

            foreach ( var valve in irrigationValves ) {
                var command = "";
                if ( valve.WateringEvents.Any( x => x.Watering ) ) {
                    var wateringEvents = valve.WateringEvents.Where( x => x.Watering ).OrderBy( x => x.EndDateTime );
                    var wateringEvent = wateringEvents.First();
                    
                    if ( wateringEvent.EndDateTime > now ) {
                        // stop watering if settings thresholds are met
                        if ( temp || rain || ( limitDict.ContainsKey( valve.ValveNumber ) && limitDict[valve.ValveNumber] ) ) {
                            wateringEvent.Watering = false;
                            wateringEvent.IrrigationValve = null;
                            wateringEvent.EndDateTime = now;
                            _wateringEventRepository.Update( wateringEvent );
                            command = "0"; // turn valve off
                        } else {
                            command = "1"; // turn valve on
                        }
                    } else {
                        wateringEvent.Watering = false;
                        wateringEvent.IrrigationValve = null;
                        _wateringEventRepository.Update( wateringEvent );
                        command = "0";
                    }
                } else if ( valve.WateringEvents.Any( x => x.StartDateTime < now && now < x.EndDateTime ) ) {
                    var wateringEvent = valve.WateringEvents.First( x => x.StartDateTime < now && now < x.EndDateTime );
                    if ( temp || rain || ( limitDict.ContainsKey( valve.ValveNumber ) && limitDict[valve.ValveNumber] ) ) {
                        wateringEvent.Watering = false;
                        wateringEvent.IrrigationValve = null;
                        wateringEvent.EndDateTime = now;
                        command = "0";
                    } else {
                        wateringEvent.Watering = true;
                        wateringEvent.IrrigationValve = null;
                        command = "1";
                    }
                    _wateringEventRepository.Update( wateringEvent );
                } else {
                    command = "0";
                }
                commandDict.Add( valve.ValveNumber.ToString(), command );
            }

            return commandDict;
        }

        /// <summary>
        /// Returns a filtered list of soil readings by FilterType used for reporting
        /// </summary>
        /// <param name="filterBy"></param>
        /// <param name="unitId"></param>
        /// <returns></returns>
        public List<object> FilterSoilReadings( FilterType filterBy, int unitId ) {
            var unit = _unitRepository.GetById( unitId );
            var data = new List<object>();
            var date = DateTimeHelper.GetLocalTime();
            switch ( filterBy ) {
                    case FilterType.Week:
                    case FilterType.Month:
                        var soilReadingsByDay = unit.SoilReadings.GroupBy( x => x.DateTime.Date ).ToDictionary( x => x.Key, x => x.ToList() );
                        var cnt = (int)filterBy;
                        date = date.AddDays( ( cnt * -1 ) );
                        for ( var i = 0; i < cnt; i++ ) {
                            data.Add( new {
                                d = date.Date.ToString( "yyyy-MM-dd" ),
                                max = soilReadingsByDay.ContainsKey( date.Date ) ? soilReadingsByDay[date.Date].Max( x => x.SoilMoisture ) * 20 : 0, // *20 to scale readings for percentages
                                min = soilReadingsByDay.ContainsKey( date.Date ) ? soilReadingsByDay[date.Date].Where( x => x.SoilMoisture > 0 ).Min( x => x.SoilMoisture ) * 20 : 0
                            } );
                            date = date.AddDays( 1 );
                        }
                        break;
                    case FilterType.Year:
                        var soilReadingsByMonth = unit.SoilReadings.GroupBy( x => x.DateTime.Month ).ToDictionary( x => x.Key, x => x.ToList() );
                        for ( var i = 1; i <= 12; i++ ) {
                            data.Add( new {
                                d = new DateTime( date.Year, i, 1 ).ToString( "yyy-MM-dd" ),
                                max = soilReadingsByMonth.ContainsKey( i ) ? soilReadingsByMonth[i].Max( x => x.SoilMoisture ) * 20 : 0,
                                min = soilReadingsByMonth.ContainsKey( i ) ? soilReadingsByMonth[i].Min( x => x.SoilMoisture ) * 20 : 0
                            } );
                            
                        }
                    break;
            }
            return data;
        }

        #endregion methods
    } // class
} // namespace
