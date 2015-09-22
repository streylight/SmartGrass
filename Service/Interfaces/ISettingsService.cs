using System.Collections.Generic;
using Core.Domains;

namespace Service.Interfaces {
    /// <summary>
    /// The interface for the soil reading service
    /// </summary>
    public interface ISettingsService {
        Settings GetSettingsById( int id );
        IList<Settings> GetAllSettings();
        void Insert( Settings unitSettings );
        void Delete( int id );
    } // class
} // namespace
