using System.Collections.Generic;
using Core.Domains;

namespace Service.Interfaces {

    public interface IUnitService {
        Unit GetUnitById(int id);
        IList<Unit> GetAllUnits();
        void Insert(Unit unit);
        void Delete(int id);
    }
}
