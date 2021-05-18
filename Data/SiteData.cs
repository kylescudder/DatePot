using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
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
		public async Task<List<PotAccess>> GetPotAccess(string UserID)
		{
			var recs = await _dataAccess.LoadData<PotAccess, dynamic>(
				"scud97_kssu.spGetPotAccess",
				new { UserID },
				"Default");
			return recs;
		}
		public async Task<int> GetUserOwnGroup(string UserID)
		{
			var recs = await _dataAccess.LoadData<int, dynamic>(
				"scud97_kssu.spGetUserGroupID",
				new { UserID },
				"Default");
			return recs[0];
		}
		public async Task<List<UserGroups>> GetUserGroups(string UserID)
		{
			var recs = await _dataAccess.LoadData<UserGroups, dynamic>(
				"scud97_kssu.spGetUserGroups",
				new { UserID },
				"Default");
			return recs;
		}
		public async Task<int> GetDefaultUserGroupID(string UserID)
		{
			var recs = await _dataAccess.LoadData<int, dynamic>(
				"scud97_kssu.spGetDefaultUserGroupID",
				new { UserID },
				"Default");
			if (recs.Count > 0)
			{
				return recs[0];
			}
			else
			{
				return 0;
			}
		}
		public Task<int> SetDefaultUserGroupID(int UserGroupID, string UserID)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("UserGroupID", UserGroupID);
			p.Add("UserID", UserID);

			return _dataAccess.SaveData(
				"scud97_kssu.spUpdateDefaultUserGroupID",
				p,
				"Default");
		}
	}
}