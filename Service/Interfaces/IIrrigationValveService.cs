using System.Collections.Generic;
using Core.Domains;

namespace Service.Interfaces {
    /// <summary>
    /// The interface for the irrigation valve service
    /// </summary>
    public interface IIrrigationValveService {
        IrrigationValve GetIrrigationValveById(int id);
        IList<IrrigationValve> GetAllIrrigationValves();
        void Insert(IrrigationValve irrigationValve);
        void Delete(int id);
    }
}
