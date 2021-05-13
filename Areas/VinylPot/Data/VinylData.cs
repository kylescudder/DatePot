using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.VinylPot.Models.Vinyls;
using System.Data;
using DatePot.Db;
using Dapper;

namespace DatePot.Areas.VinylPot.Data
{
    public class VinylData : IVinylData
	{
		private readonly ISqlDb _dataAccess;
		public VinylData(ISqlDb dataAccess
			//, IHttpContextAccessor httpContextAccessor
			)
		{
			_dataAccess = dataAccess;
			//_httpContextAccessor = httpContextAccessor;
		}

		public async Task<List<VinylDetails>> GetVinylDetails(int VinylID)
		{
			var recs = await _dataAccess.LoadData<VinylDetails, dynamic>(
				"scud97_kssu.spGetVinylDetails",
				new { VinylID },
				"Default");
			return recs;
		}
		public Task<int> UpdateVinyl(int VinylID, string Name, string ArtistName, bool Purchased, int AddedByID)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("VinylID", VinylID);
			p.Add("Name", Name);
			p.Add("ArtistName", ArtistName);
			p.Add("Purchased", Purchased);
			p.Add("AddedByID", AddedByID);

			return _dataAccess.SaveData(
				"scud97_kssu.spUpdateVinyl",
				p,
				"Default");
		}
		public Task<int> ArchiveVinyl(int VinylID)
		{
			return _dataAccess.SaveData(
				"scud97_kssu.spArchiveVinyl",
				new { VinylID },
				"Default");
		}
		public async Task<List<VinylList>> GetVinylList()
		{
			var recs = await _dataAccess.LoadData<VinylList, dynamic>(
				"scud97_kssu.spGetVinylList",
				new { },
				"Default");
			return recs;
		}
		public async Task<int> AddVinyl(string Name, string ArtistName, bool Purchased, int AddedByID)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("Name", Name);
			p.Add("ArtistName", ArtistName);
			p.Add("Purchased", Purchased);
			p.Add("AddedByID", AddedByID);
			p.Add("VinylID", DbType.Int32, direction: ParameterDirection.Output);

			await _dataAccess.SaveData("scud97_kssu.spAddVinyl", p, "Default");

			return p.Get<int>("VinylID");
		}
	}
}
