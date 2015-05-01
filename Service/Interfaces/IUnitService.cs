using System.Collections.Generic;
using Core.Domains;

namespace Service.Interfaces {
    /// <summary>
    /// The interface for the unit service
    /// </summary>
    public interface IUnitService {
        Unit GetUnitById(int id);
        IList<Unit> GetAllUnits();
        void Insert(Unit unit);
        void Delete(int id);
        int ValidateProductKey(string productKey);
        Dictionary<string, string> GetValveCommands(int id, Dictionary<int, bool> limitDict, bool temp, bool rain);
    }
}
