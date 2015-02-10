using System.Collections.Generic;
using Core.Domains;

namespace Service.Interfaces {

    public interface IIrrigationValveService {
        IrrigationValve GetIrrigationValveById(int id);
        IList<IrrigationValve> GetAllIrrigationValves();
        void Insert(IrrigationValve irrigationValve);
        void Delete(int id);
    }
}
