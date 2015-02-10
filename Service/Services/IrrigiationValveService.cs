using System;
using System.Collections.Generic;
using System.Linq;
using Core.Domains;
using Map.Repo;

namespace Service.Interfaces {
    public class IrrigationValveService : IIrrigationValveService{

        private readonly IRepository<IrrigationValve> _irrigationValveRepository;

        public IrrigationValveService(){
            _irrigationValveRepository = new Repository<IrrigationValve>();    
        }

        public IrrigationValve GetIrrigationValveById(int id){
            return _irrigationValveRepository.GetById(id);
        }

        public IList<IrrigationValve> GetAllIrrigationValves(){
            return _irrigationValveRepository.Table.ToList();
        }

        public void Insert(IrrigationValve irrigationValve){
            try{
                if (irrigationValve.Id == 0)
                    _irrigationValveRepository.Insert(irrigationValve);
                else
                    _irrigationValveRepository.Update(irrigationValve);    

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id){
            try{
                var irrigationValve = _irrigationValveRepository.GetById(id);
                _irrigationValveRepository.Delete(irrigationValve);

            } catch (Exception ex){
                throw new Exception(ex.Message);
            }
        }
    }
}
