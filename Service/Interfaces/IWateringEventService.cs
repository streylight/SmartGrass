using System.Collections.Generic;
using Core.Domains;

namespace Service.Interfaces {
    /// <summary>
    /// The interface for the watering event service
    /// </summary>
    public interface IWateringEventService {
        WateringEvent GetWateringEventById( int id );
        IList<WateringEvent> GetAllWateringEvents();
        void Insert( WateringEvent wateringEvent );
        void Delete( int id );
    } // class
} // namespace
