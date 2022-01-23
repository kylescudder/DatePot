using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.ActivityPot.Models.Activitys;
using System.Data;
using DatePot.Db;
using Dapper;

namespace DatePot.Areas.ActivityPot.Data
{
	public class ActivityData : IActivityData
	{
		private readonly ISqlDb _dataAccess;
		public ActivityData(ISqlDb dataAccess)
		{
			_dataAccess = dataAccess;
		}
		public List<RandomActivity> RandomActivitys { get; set; }
		public async Task<List<ActivityDetails>> GetActivityDetails(int ActivityID)
		{
			var recs = await _dataAccess.LoadData<ActivityDetails, dynamic>(
				"scud97_kssu.spGetActivityDetails",
				new { ActivityID },
				"Default");
			return recs;
		}
		public async Task<List<ActivityTypes>> GetyActivityTypeList()
		{
			var recs = await _dataAccess.LoadData<ActivityTypes, dynamic>(
				"scud97_kssu.spGetActivityTypeList",
				new { },
				"Default");
			return recs;
		}
		public Task<int> UpdateActivity(int ActivityID, string ActivityName, string Location, int ExpenseID, string Description, bool Prebook)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("ActivityID", ActivityID);
			p.Add("ActivityName", ActivityName);
			p.Add("Location", Location);
			p.Add("ExpenseID", ExpenseID);
			p.Add("Description", Description);
			p.Add("Prebook", Prebook);

			return _dataAccess.SaveData(
				"scud97_kssu.spUpdateActivity",
				p,
				"Default");
		}
		public Task<int> ArchiveActivity(int ActivityID)
		{
			return _dataAccess.SaveData(
				"scud97_kssu.spArchiveActivity",
				new { ActivityID },
				"Default");
		}
		public async Task<List<ActivityList>> GetActivityList(int? UserGroupID)
		{
			var recs = await _dataAccess.LoadData<ActivityList, dynamic>(
				"scud97_kssu.spGetActivityList",
				new { UserGroupID },
				"Default");
			return recs;
		}
		public async Task<int> AddActivity(string ActivityName, string Location, int? ExpenseID, string Description, bool Prebook, int? UserGroupID)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("ActivityName", ActivityName);
			p.Add("Location", Location);
			p.Add("Description", Description);
			p.Add("ExpenseID", ExpenseID);
			p.Add("Prebook", Prebook);
			p.Add("UserGroupID", UserGroupID);
			p.Add("ActivityID", DbType.Int32, direction: ParameterDirection.Output);

			await _dataAccess.SaveData("scud97_kssu.spAddActivity", p, "Default");

			return p.Get<int>("ActivityID");
		}
		public Task<int> AddGenre(string GenreText)
		{
			return _dataAccess.SaveData(
				"scud97_kssu.spAddGenre",
				new { GenreText },
				"Default");
		}
		public Task<int> AddDirector(string DirectorText)
		{
			return _dataAccess.SaveData(
				"scud97_kssu.spAddDirector",
				new { DirectorText },
				"Default");
		}
		public Task<int> ActivityWatched(int ActivityID)
		{
			return _dataAccess.SaveData(
				"scud97_kssu.spActivityWatched",
				new { ActivityID },
				"Default");
		}
		public async Task<List<Expenses>> GetExpenseList()
		{
			var recs = await _dataAccess.LoadData<Expenses, dynamic>(
				"scud97_kssu.spGetExpenseList",
				new { },
				"Default");
			return recs;
		}
		public async Task<bool> ActivityTypeDupeCheck(string ActivityTypeText)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("ActivityTypeText", ActivityTypeText);
			p.Add("ActivityTypeExists", DbType.Int32, direction: ParameterDirection.Output);

			await _dataAccess.SaveData("scud97_kssu.spActivityTypeDupeCheck", p, "Default");

			var i = p.Get<int>("ActivityTypeExists");
			return Convert.ToBoolean(i);
		}
		public Task<int> AddActivityType(string ActivityTypeText)
		{
			return _dataAccess.SaveData(
				"scud97_kssu.spAddActivityType",
				new { ActivityTypeText },
				"Default");
		}
		public Task<int> AddActivityxType(int ActivityID, int ActivityTypeID)
		{
			return _dataAccess.SaveData(
				"scud97_kssu.spAddActivityxType",
				new { ActivityID, ActivityTypeID },
				"Default");
		}
		public async Task<List<ActivityxTypes>> GetActivityxTypes(int ActivityID)
		{
			var recs = await _dataAccess.LoadData<ActivityxTypes, dynamic>(
				"scud97_kssu.spGetActivityxTypes",
				new { ActivityID },
				"Default");
			return recs;
		}
		public Task<int> DeleteActivityxType(int ActivityxTypeID)
		{
			return _dataAccess.SaveData(
				"scud97_kssu.spDeleteActivityxType",
				new { ActivityxTypeID },
				"Default");
		}
		public async Task<List<RandomActivity>> GetRandomActivity(int ActivityTypeID, int? UserGroupID)
		{
			DynamicParameters p = new DynamicParameters();
			p.Add("ActivityTypeID", ActivityTypeID);
			p.Add("UserGroupID", UserGroupID);

			var recs = await _dataAccess.LoadData<RandomActivity, dynamic>(
					"scud97_kssu.spRandomActivity",
					p,
					"Default");
			return recs;
		}
	}
}
