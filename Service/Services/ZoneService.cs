using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    public class ZoneService : IZoneService{

        private readonly IRepository<Zone> _zoneRepository;

        public ZoneService(){
            _zoneRepository = new Repository<Zone>();    
        }

        public Zone GetZoneById(int id){
            return _zoneRepository.GetById(id);
        }

        public IList<Zone> GetAllZones(){
            return _zoneRepository.Table.ToList();
        }

        public void Insert(Zone zone){
            try{
                if (zone.Id == 0)
                    _zoneRepository.Insert(zone);
                else
                    _zoneRepository.Update(zone);    

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id){
            try{
                var zone = _zoneRepository.GetById(id);
                _zoneRepository.Delete(zone);

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }
    }
}
