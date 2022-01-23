using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.ActivityPot.Models.Activitys;

namespace DatePot.Areas.ActivityPot.Data
{
	public interface IActivityData
	{
		List<RandomActivity> RandomActivitys { get; set; }

		Task<bool> ActivityTypeDupeCheck(string ActivityTypeText);
		Task<int> ActivityWatched(int ActivityID);
		Task<int> AddActivity(string ActivityName, string Location, int? ExpenseID, string Description, bool Prebook, int? UserGroupID);
		Task<int> AddActivityType(string ActivityTypeText);
		Task<int> AddActivityxType(int ActivityID, int ActivityTypeID);
		Task<int> AddDirector(string DirectorText);
		Task<int> AddGenre(string GenreText);
		Task<int> ArchiveActivity(int ActivityID);
		Task<int> DeleteActivityxType(int ActivityxTypeID);
		Task<List<ActivityDetails>> GetActivityDetails(int ActivityID);
		Task<List<ActivityList>> GetActivityList(int? UserGroupID);
		Task<List<ActivityxTypes>> GetActivityxTypes(int ActivityID);
		Task<List<Expenses>> GetExpenseList();
		Task<List<RandomActivity>> GetRandomActivity(int ActivityTypeID, int? UserGroupID);
		Task<List<ActivityTypes>> GetyActivityTypeList();
		Task<int> UpdateActivity(int ActivityID, string ActivityName, string Location, int ExpenseID, string Description, bool Prebook);
	}
}
