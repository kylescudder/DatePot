using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.VinylPot.Models.Vinyls;

namespace DatePot.Areas.VinylPot.Data
{
	public interface IVinylData
	{
		Task<int> AddVinyl(string Name, string ArtistName, bool Purchased, string AddedByID, int? UserGroupID);
		Task<int> ArchiveVinyl(int VinylID);
		Task<List<VinylDetails>> GetVinylDetails(int VinylID);
		Task<List<VinylList>> GetVinylList(int? UserGroupID);
		Task<int> UpdateVinyl(int VinylID, string Name, string ArtistName, bool Purchased, int AddedByID);
	}
}
