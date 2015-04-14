using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    /// <summary>
    /// The service for the rain event class
    /// </summary>
    public class RainEventService : IRainEventService{

        private readonly IRepository<RainEvent> _rainEventRepository;

        public RainEventService(){
            _rainEventRepository = new Repository<RainEvent>();    
        }

        public RainEvent GetRainEventById(int id){
            return _rainEventRepository.GetById(id);
        }

        public IList<RainEvent> GetAllRainEvents(){
            return _rainEventRepository.Table.ToList();
        }

        public void Insert(RainEvent rainEvent){
            try{
                if (rainEvent.Id == 0)
                    _rainEventRepository.Insert(rainEvent);
                else
                    _rainEventRepository.Update(rainEvent);    

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id){
            try{
                var rainEvent = _rainEventRepository.GetById(id);
                _rainEventRepository.Delete(rainEvent);

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }
    }
}
