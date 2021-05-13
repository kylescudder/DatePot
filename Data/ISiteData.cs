using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Models.Site;

namespace DatePot.Data
{
    public interface ISiteData
    {
        Task<List<PotAccess>> GetPotAccess(int UserID);
    }
}

