using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the watering event class
    /// </summary>
    public class WateringEventService : IWateringEventService {

        #region vars
        
        private readonly IRepository<WateringEvent> _wateringEventRepository;

        #endregion

        #region ctor

        /// <summary>
        /// Constructor for the WateringEvent class
        /// </summary>
        public WateringEventService() {
            _wateringEventRepository = new Repository<WateringEvent>();    
        }

        #endregion

        #region methods

        /// <summary>
        /// Returns a WateringEvent with matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public WateringEvent GetWateringEventById( int id ) {
            return _wateringEventRepository.GetById( id );
        }

        /// <summary>
        /// Returns all WateringEvents
        /// </summary>
        /// <returns></returns>
        public IList<WateringEvent> GetAllWateringEvents() {
            return _wateringEventRepository.Table.ToList();
        }

        /// <summary>
        /// Inserts a new WateringEvent into the db
        /// </summary>
        /// <param name="wateringEvent"></param>
        public void Insert( WateringEvent wateringEvent ) {
            if ( wateringEvent.Id == 0 ) {
                _wateringEventRepository.Insert( wateringEvent );
            } else {
                _wateringEventRepository.Update( wateringEvent );
            }
        }

        /// <summary>
        /// Deletes a WateringEvent from the db
        /// </summary>
        /// <param name="id"></param>
        public void Delete( int id ) {
            var wateringEvent = _wateringEventRepository.GetById( id );
            if ( wateringEvent == null ) {
                throw new Exception( string.Format( "No temperature reading found with id: {0}", id ) ); 
            }
            _wateringEventRepository.Delete( wateringEvent );
        }

        #endregion
    } // class
} // namespace
