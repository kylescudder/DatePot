using DatePot.Areas.FoodPot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatePot.Areas.FoodPot.Data
{
    public interface IFoodData
    {
        List<Food.RandomRestaurant> RandomRestaurants { get; set; }

        Task<int> AddFoodType(string FoodTypeText);
        Task<int> AddRestaurant(string RestaurantName, int ExpenseID, int LocationID);
        Task<int> AddRestaurantFoodType(int RestaurantID, int FoodTypeID);
        Task<int> AddRestaurantWhen(int RestaurantID, int WhenID);
        Task<int> AddWhen(string WhenText);
        Task<int> ArchiveRestaurant(int RestaurantID);
        Task<int> DeleteRestaurantFoodType(int RestaurantFoodTypeID);
        Task<int> DeleteRestaurantWhen(int RestaurantWhenID);
        Task<bool> FoodTypeDupeCheck(string FoodTypeText);
        Task<List<Food.Expenses>> GetExpenseList();
        Task<List<Food.FoodTypes>> GetFoodTypeList();
        Task<List<Food.Locations>> GetLocationList();
        Task<List<Food.RandomRestaurant>> GetRandomRestaurant(int WhenID);
        Task<List<Food.RestaurantDetails>> GetRestaurantDetails(int RestaurantID);
        Task<List<Food.RestaurantFoodTypes>> GetRestaurantFoodTypes(int RestaurantID);
        Task<List<Food.RestaurantList>> GetRestaurantList();
        Task<List<Food.RestaurantWhens>> GetRestaurantWhens(int RestaurantID);
        Task<List<Food.UserList>> GetUserList();
        Task<List<Food.When>> GetWhenList();
        Task<int> RestaurantWatched(int RestaurantID);
        Task<int> UpdateRestaurant(int RestaurantID, string RestaurantName, int ExpenseID, int LocationID);
        Task<bool> WhenDupeCheck(string WhenText);
    }
}