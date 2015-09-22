using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the temperature reading class
    /// </summary>
    public class TemperatureReadingService : ITemperatureReadingService {

        #region vars

        private readonly IRepository<TemperatureReading> _temperatureReadingRepository;

        #endregion

        #region ctor

        /// <summary>
        /// Constructor for the TemperatureReadingService class
        /// </summary>
        public TemperatureReadingService() {
            _temperatureReadingRepository = new Repository<TemperatureReading>();    
        }

        #endregion

        #region methods

        /// <summary>
        /// Returns a TemperatureReading with matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TemperatureReading GetTemperatureReadingById( int id ) {
            return _temperatureReadingRepository.GetById(id);
        }

        /// <summary>
        /// Returns all TemperatureReadings
        /// </summary>
        /// <returns></returns>
        public IList<TemperatureReading> GetAllTemperatureReadings() {
            return _temperatureReadingRepository.Table.ToList();
        }

        /// <summary>
        /// Inserts a new TemperatureReading into the db
        /// </summary>
        /// <param name="temperatureReading"></param>
        public void Insert( TemperatureReading temperatureReading ) {
            if ( temperatureReading.Id == 0 ) {
                _temperatureReadingRepository.Insert( temperatureReading );
            } else {
                _temperatureReadingRepository.Update( temperatureReading );
            }
        }

        /// <summary>
        /// Deletes a TemperatureReading from the db
        /// </summary>
        /// <param name="id"></param>
        public void Delete(int id){
            var temperatureReading = _temperatureReadingRepository.GetById( id );
            if ( temperatureReading == null ) {
                throw new Exception(string.Format("No temperature reading found with id: {0}", id)); 
            }
            _temperatureReadingRepository.Delete(temperatureReading);
        }

        #endregion
    } // class
} // namespace
