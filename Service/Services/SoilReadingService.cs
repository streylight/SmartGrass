using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Core.Helpers;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the soil reading class
    /// </summary>
    public class SoilReadingService : ISoilReadingService {

        #region vars

        private readonly IRepository<SoilReading> _soilReadingRepository;

        #endregion

        #region ctor

        /// <summary>
        /// Constructor for the SoilReadingService class
        /// </summary>
        public SoilReadingService() {
            _soilReadingRepository = new Repository<SoilReading>();    
        }

        #endregion

        #region methods
        
        /// <summary>
        /// Returns a SoilReading with matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SoilReading GetSoilReadingById( int id ) {
            return _soilReadingRepository.GetById( id );
        }

        /// <summary>
        /// Returns all SoilReadings
        /// </summary>
        /// <returns></returns>
        public IList<SoilReading> GetAllSoilReadings() {
            return _soilReadingRepository.Table.ToList();
        }

        /// <summary>
        /// Inserts a list of new SoilReadings into the db 
        /// </summary>
        /// <param name="soilReadings"></param>
        /// <param name="unitId"></param>
        public void Insert( List<SoilReading> soilReadings, int unitId ) {
            var now = DateTimeHelper.GetLocalTime();
            foreach ( var soilReading in soilReadings ) {
                if ( soilReading.Id == 0 ) {
                    soilReading.DateTime = now;
                    soilReading.UnitId = unitId;
                    _soilReadingRepository.Insert( soilReading );
                }
            }
        }

        /// <summary>
        /// Deletes a SoilReading from the db
        /// </summary>
        /// <param name="id"></param>
        public void Delete( int id ) {
            var soilReading = _soilReadingRepository.GetById( id );
            if ( soilReading == null ) {
                throw new Exception( string.Format( "No rain event found with id: {0}", id ) );
            }
            _soilReadingRepository.Delete( soilReading );
        }

        #endregion
    } // class
} // namespace
