using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the irrigation valve class
    /// </summary>
    public class IrrigationValveService : IIrrigationValveService {

        #region vars
        
        private readonly IRepository<IrrigationValve> _irrigationValveRepository;
        
        #endregion

        #region ctor

        /// <summary>
        /// Constructor for the IrrigationValveService class
        /// </summary>
        public IrrigationValveService() {
            _irrigationValveRepository = new Repository<IrrigationValve>();    
        }

        #endregion

        #region methods

        /// <summary>
        /// Returns an IrrigationValve with a matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IrrigationValve GetIrrigationValveById( int id ) {
            return _irrigationValveRepository.GetById( id );
        }

        /// <summary>
        /// Returns all IrrigationValves
        /// </summary>
        /// <returns></returns>
        public IList<IrrigationValve> GetAllIrrigationValves() {
            return _irrigationValveRepository.Table.ToList();
        }

        /// <summary>
        /// Inserts a new IrrigationValve in the db
        /// </summary>
        /// <param name="irrigationValve"></param>
        public void Insert( IrrigationValve irrigationValve ) {
            if ( irrigationValve.Id == 0 ) {
                _irrigationValveRepository.Insert( irrigationValve );
            } else {
                _irrigationValveRepository.Update( irrigationValve );
            }
        }

        /// <summary>
        /// Deletes an IrrigationValve from the db
        /// </summary>
        /// <param name="id"></param>
        public void Delete( int id ) {
            var irrigationValve = _irrigationValveRepository.GetById( id );
            if ( irrigationValve == null ) {
                throw new Exception( string.Format( "No irrigation valve found with id: {0}", id ) );
            }
            _irrigationValveRepository.Delete( irrigationValve );
        }

        #endregion
    } // class
} // namespace
