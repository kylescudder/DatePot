using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.FoodPot.Models.Food;

namespace DatePot.Areas.FoodPot.Data
{
	public interface IFoodData
	{
		List<RandomRestaurant> RandomRestaurants { get; set; }

		Task<int> AddFoodType(string FoodTypeText);
		Task<int> AddRestaurant(string RestaurantName, int ExpenseID, int LocationID, int? UserGroupID);
		Task<int> AddRestaurantFoodType(int RestaurantID, int FoodTypeID);
		Task<int> AddRestaurantWhen(int RestaurantID, int WhenID);
		Task<int> AddWhen(string WhenText);
		Task<int> ArchiveRestaurant(int RestaurantID);
		Task<int> DeleteRestaurantFoodType(int RestaurantFoodTypeID);
		Task<int> DeleteRestaurantWhen(int RestaurantWhenID);
		Task<bool> FoodTypeDupeCheck(string FoodTypeText);
		Task<List<Expenses>> GetExpenseList();
		Task<List<FoodTypes>> GetFoodTypeList();
		Task<List<Locations>> GetLocationList();
		Task<List<RandomRestaurant>> GetRandomRestaurant(int WhenID, int? UserGroupID);
		Task<List<RestaurantDetails>> GetRestaurantDetails(int RestaurantID);
		Task<List<RestaurantFoodTypes>> GetRestaurantFoodTypes(int RestaurantID);
		Task<List<RestaurantList>> GetRestaurantList(int? UserGroupID);
		Task<List<RestaurantWhens>> GetRestaurantWhens(int RestaurantID);
		Task<List<UserList>> GetUserList(int? UserGroupID);
		Task<List<When>> GetWhenList();
		Task<int> RestaurantWatched(int RestaurantID);
		Task<int> UpdateRestaurant(int RestaurantID, string RestaurantName, int ExpenseID, int LocationID);
		Task<bool> WhenDupeCheck(string WhenText);
	}
}
