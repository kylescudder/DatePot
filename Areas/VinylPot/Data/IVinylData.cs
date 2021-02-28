using System.Collections.Generic;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using static DatePot.Areas.VinylPot.Models.Vinyls;

namespace DatePot.Areas.VinylPot.Data
{
    public interface IVinylData
    {
        Task<int> AddVinyl(string VinylName, string VinylArtistName, bool VinylPurchased);
        Task<int> ArchiveVinyl(int VinylID);
        Task<List<VinylDetails>> GetVinylDetails(int VinylID);
        Task<List<VinylList>> GetVinylList();
        Task<int> UpdateVinyl(int VinylID, string VinylName, string VinylArtistName, bool VinylPurchased);
    }
}
