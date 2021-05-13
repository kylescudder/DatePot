using System.Collections.Generic;
using System.Threading.Tasks;
using DatePot.Db;
using static DatePot.Models.Site;

namespace DatePot.Data
{
    public class SiteData : ISiteData
    {
        private readonly ISqlDb _dataAccess;
        public SiteData(ISqlDb dataAccess
            )
        {
            _dataAccess = dataAccess;
        }
        public async Task<List<PotAccess>> GetPotAccess(int UserID)
        {
            var recs = await _dataAccess.LoadData<PotAccess, dynamic>(
                "scud97_kssu.spGetPotAccess",
                new { UserID },
                "Default");
            return recs;
        }
    }
}

