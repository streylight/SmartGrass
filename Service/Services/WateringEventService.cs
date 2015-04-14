using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the watering event class
    /// </summary>
    public class WateringEventService : IWateringEventService{

        private readonly IRepository<WateringEvent> _wateringEventRepository;

        public WateringEventService(){
            _wateringEventRepository = new Repository<WateringEvent>();    
        }

        public WateringEvent GetWateringEventById(int id){
            return _wateringEventRepository.GetById(id);
        }

        public IList<WateringEvent> GetAllWateringEvents(){
            return _wateringEventRepository.Table.ToList();
        }

        public void Insert(WateringEvent wateringEvent){
            try{
                if (wateringEvent.Id == 0)
                    _wateringEventRepository.Insert(wateringEvent);
                else
                    _wateringEventRepository.Update(wateringEvent);    

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id){
            try{
                var wateringEvent = _wateringEventRepository.GetById(id);
                _wateringEventRepository.Delete(wateringEvent);

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }
    }
}
