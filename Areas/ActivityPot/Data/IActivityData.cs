using DatePot.Areas.ActivityPot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatePot.Areas.ActivityPot.Data
{
    public interface IActivityData
    {
        Task<bool> ActivityTypeDupeCheck(string ActivityTypeText);
        Task<int> ActivityWatched(int ActivityID);
        Task<int> AddActivity(string ActivityName, string Location, int? ExpenseID, string Description, bool Prebook);
        Task<int> AddActivityType(string ActivityTypeText);
        Task<int> AddActivityxType(int ActivityID, int ActivityTypeID);
        Task<int> AddDirector(string DirectorText);
        Task<int> AddGenre(string GenreText);
        Task<int> ArchiveActivity(int ActivityID);
        Task<int> DeleteActivityxType(int ActivityxTypeID);
        Task<List<Activitys.ActivityDetails>> GetActivityDetails(int ActivityID);
        Task<List<Activitys.ActivityList>> GetActivityList();
        Task<List<Activitys.ActivityxTypes>> GetActivityxTypes(int ActivityID);
        Task<List<Activitys.Expenses>> GetExpenseList();
        Task<List<Activitys.RandomActivity>> GetRandomActivity();
        Task<List<Activitys.ActivityTypes>> GetyActivityTypeList();
        Task<int> UpdateActivity(int ActivityID, string ActivityName, string Location, int ExpenseID, string Description, bool Prebook);
    }
}