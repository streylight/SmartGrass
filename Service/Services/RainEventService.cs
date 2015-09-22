using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the rain event class
    /// </summary>
    public class RainEventService : IRainEventService {

        #region vars

        private readonly IRepository<RainEvent> _rainEventRepository;

        #endregion

        #region ctor

        /// <summary>
        /// Constructor for the RainEventService class
        /// </summary>
        public RainEventService() {
            _rainEventRepository = new Repository<RainEvent>();    
        }

        #endregion

        #region methods

        /// <summary>
        /// Returns a RainEvent with matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public RainEvent GetRainEventById( int id ) {
            return _rainEventRepository.GetById( id );
        }

        /// <summary>
        /// Returns all RainEvents
        /// </summary>
        /// <returns></returns>
        public IList<RainEvent> GetAllRainEvents() {
            return _rainEventRepository.Table.ToList();
        }

        /// <summary>
        /// Inserts a new RainEvent into the db
        /// </summary>
        /// <param name="rainEvent"></param>
        public void Insert( RainEvent rainEvent ) {
            if (rainEvent.Id == 0) {
                _rainEventRepository.Insert( rainEvent );
            } else {
                _rainEventRepository.Update( rainEvent );
            }
        }

        /// <summary>
        /// Deletes a RainEvent from the db
        /// </summary>
        /// <param name="id"></param>
        public void Delete( int id ) {
            var rainEvent = _rainEventRepository.GetById( id );
            if ( rainEvent == null ) {
                throw new Exception( string.Format( "No rain event found with id: {0}", id ) );
            }
            _rainEventRepository.Delete( rainEvent );
        }

        #endregion
    } // class
} // namespace
