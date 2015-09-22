using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;
using Service.Interfaces;

namespace Service.Services {
    /// <summary>
    /// The service for the soil reading class
    /// </summary>
    public class SettingsService : ISettingsService {

        #region vars

        private readonly IRepository<Settings> _settingsRepository;

        #endregion

        #region ctor

        /// <summary>
        /// Constructor for the Settings class
        /// </summary>
        public SettingsService() {
            _settingsRepository = new Repository<Settings>();    
        }

        #endregion

        #region methods

        /// <summary>
        /// Returns a Setting with matching id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Settings GetSettingsById( int id ) {
            return _settingsRepository.GetById( id );
        }

        /// <summary>
        /// Returns all Settings
        /// </summary>
        /// <returns></returns>
        public IList<Settings> GetAllSettings() {
            return _settingsRepository.Table.ToList();
        }

        /// <summary>
        /// Inserts a new Settings object into the db
        /// </summary>
        /// <param name="unitSettings"></param>
        public void Insert( Settings unitSettings ) {
            if ( unitSettings.Id == 0 ) {
                _settingsRepository.Insert( unitSettings );
            } else {
                _settingsRepository.Update( unitSettings );
            }
        }

        /// <summary>
        /// Deletes a Setting from the db
        /// </summary>
        /// <param name="id"></param>
        public void Delete( int id ) {
            var settings = _settingsRepository.GetById(id);
            if ( settings == null ) {
                throw new Exception( string.Format( "No settings found with id: {0}", id ) );
            }
            _settingsRepository.Delete( settings );
        }

        #endregion
    } // class
} // namespace
