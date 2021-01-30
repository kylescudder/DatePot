using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatePot.Db
{
    public interface ISqlDb
    {
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        Task<int> SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
    }
}