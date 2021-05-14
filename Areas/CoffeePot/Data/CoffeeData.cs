using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.CoffeePot.Models.Coffees;
using System.Data;
using DatePot.Db;
using Dapper;

namespace DatePot.Areas.CoffeePot.Data
{
	public class CoffeeData : ICoffeeData
	{
		private readonly ISqlDb _dataAccess;
		public CoffeeData(ISqlDb dataAccess
			)
		{
			_dataAccess = dataAccess;
		}

		public async Task<List<CoffeeDetails>> GetCoffeeDetails(int CoffeeID)
		{
			var recs = await _dataAccess.LoadData<CoffeeDetails, dynamic>(
				"scud97_kssu.spGetCoffeeDetails",
				new { CoffeeID },
				"Default");
			return recs;
		}
		public Task<int> UpdateCoffee(int CoffeeID, string CoffeeName, int KyleTaste, int RhiannTaste, int KyleExperience, int RhiannExperience)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("CoffeeID", CoffeeID);
			p.Add("CoffeeName", CoffeeName);
			p.Add("KyleTaste", KyleTaste);
			p.Add("RhiannTaste", RhiannTaste);
			p.Add("KyleExperience", KyleExperience);
			p.Add("RhiannExperience", RhiannExperience);

			return _dataAccess.SaveData(
				"scud97_kssu.spUpdateCoffee",
				p,
				"Default");
		}
		public Task<int> ArchiveCoffee(int CoffeeID)
		{
			return _dataAccess.SaveData(
				"scud97_kssu.spArchiveCoffee",
				new { CoffeeID },
				"Default");
		}
		public async Task<List<CoffeeList>> GetCoffeeList(int? UserGroupID)
		{

			var recs = await _dataAccess.LoadData<CoffeeList, dynamic>(
				"scud97_kssu.spGetCoffeeList",
				new { UserGroupID },
				"Default");
			return recs;
		}
		public async Task<int> AddCoffee(string CoffeeName, int? UserGroupID)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("CoffeeName", CoffeeName);
			p.Add("UserGroupID", UserGroupID);
			p.Add("CoffeeID", DbType.Int32, direction: ParameterDirection.Output);

			await _dataAccess.SaveData("scud97_kssu.spAddCoffee", p, "Default");

			return p.Get<int>("CoffeeID");
		}
		public async Task<List<RandomCoffee>> GetRandomCoffee(int? UserGroupID)
		{
			var recs = await _dataAccess.LoadData<RandomCoffee, dynamic>(
				"scud97_kssu.spRandomCoffee",
				new { UserGroupID },
				"Default");
			return recs;
		}
	}
}
