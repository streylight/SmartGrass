using System.Collections.Generic;
using Core.Domains;

namespace Service.Interfaces {
    /// <summary>
    /// The interface for the rain event service
    /// </summary>
    public interface IRainEventService{
        RainEvent GetRainEventById(int id);
        IList<RainEvent> GetAllRainEvents();
        void Insert(RainEvent rainEvent);
        void Delete(int id);
    }
}
