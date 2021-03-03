using System.Collections.Generic;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using static DatePot.Areas.VinylPot.Models.Vinyls;

namespace DatePot.Areas.VinylPot.Data
{
	public interface IVinylData
	{
		Task<int> AddVinyl(string Name, string ArtistName, bool Purchased, int AddedByID);
		Task<int> ArchiveVinyl(int VinylID);
		Task<List<VinylDetails>> GetVinylDetails(int VinylID);
		Task<List<VinylList>> GetVinylList();
		Task<int> UpdateVinyl(int VinylID, string Name, string ArtistName, bool Purchased, int AddedByID);
	}
}
