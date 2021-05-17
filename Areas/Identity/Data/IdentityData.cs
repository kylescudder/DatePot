using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.Identity.Models.Identity;
using DatePot.Db;
using Dapper;

namespace DatePot.Areas.Identity.Data
{
    public class IdentityData : IIdentityData
    {
        private readonly ISqlDb _dataAccess;
        public IdentityData(ISqlDb dataAccess
            )
        {
            _dataAccess = dataAccess;
        }
        public Task<int> AddUser(string Name, string Id)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("Name", Name);
            p.Add("Id", Id);

            return _dataAccess.SaveData(
                "scud97_kssu.spAddUserName",
                p,
                "Default");
        }
        public async Task<List<UserList>> GetUser(string UserID)
        {
            var recs = await _dataAccess.LoadData<UserList, dynamic>(
                "scud97_kssu.spGetUser",
                new { UserID },
                "Default");
            return recs;
        }
    }
}

