using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DatePot.Areas.FoodPot.Models.Food;
using System.Data;
using DatePot.Db;
using Dapper;

namespace DatePot.Areas.FoodPot.Data
{
    public class FoodData : IFoodData
    {
        private readonly ISqlDb _dataAccess;
        public FoodData(ISqlDb dataAccess
            )
        {
            _dataAccess = dataAccess;
        }
        public List<RandomRestaurant> RandomRestaurants { get; set; }
        public async Task<List<RestaurantDetails>> GetRestaurantDetails(int RestaurantID)
        {
            var recs = await _dataAccess.LoadData<RestaurantDetails, dynamic>(
                "scud97_kssu.spGetRestaurantDetails",
                new { RestaurantID },
                "Default");
            return recs;
        }
        public async Task<List<FoodTypes>> GetFoodTypeList()
        {
            var recs = await _dataAccess.LoadData<FoodTypes, dynamic>(
                "scud97_kssu.spGetFoodTypeList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<When>> GetWhenList()
        {
            var recs = await _dataAccess.LoadData<When, dynamic>(
                "scud97_kssu.spGetWhenList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<Expenses>> GetExpenseList()
        {
            var recs = await _dataAccess.LoadData<Expenses, dynamic>(
                "scud97_kssu.spGetExpenseList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<Locations>> GetLocationList()
        {
            var recs = await _dataAccess.LoadData<Locations, dynamic>(
                "scud97_kssu.spGetLocationList",
                new { },
                "Default");
            return recs;
        }

        public Task<int> UpdateRestaurant(int RestaurantID, string RestaurantName, int ExpenseID, int LocationID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("RestaurantID", RestaurantID);
            p.Add("RestaurantName", RestaurantName);
            p.Add("ExpenseID", ExpenseID);
            p.Add("LocationID", LocationID);

            return _dataAccess.SaveData(
                "scud97_kssu.spUpdateRestaurant",
                p,
                "Default");
        }
        public Task<int> ArchiveRestaurant(int RestaurantID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spArchiveRestaurant",
                new { RestaurantID },
                "Default");
        }
        public async Task<List<RestaurantList>> GetRestaurantList()
        {
            var recs = await _dataAccess.LoadData<RestaurantList, dynamic>(
                "scud97_kssu.spGetRestaurantList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<UserList>> GetUserList()
        {
            var recs = await _dataAccess.LoadData<UserList, dynamic>(
                "scud97_kssu.spGetUserList",
                new { },
                "Default");
            return recs;
        }

        public async Task<int> AddRestaurant(string RestaurantName, int ExpenseID, int LocationID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("RestaurantName", RestaurantName);
            p.Add("ExpenseID", ExpenseID);
            p.Add("LocationID", LocationID);
            p.Add("RestaurantID", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spAddRestaurant", p, "Default");

            return p.Get<int>("RestaurantID");
        }
        public Task<int> AddFoodType(string FoodTypeText)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spAddFoodType",
                new { FoodTypeText },
                "Default");
        }
        public Task<int> AddWhen(string WhenText)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spAddWhen",
                new { WhenText },
                "Default");
        }
        public async Task<bool> FoodTypeDupeCheck(string FoodTypeText)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("FoodTypeText", FoodTypeText);
            p.Add("FoodTypeExists", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spFoodTypeDupeCheck", p, "Default");

            var i = p.Get<int>("FoodTypeExists");
            return Convert.ToBoolean(i);
        }
        public async Task<bool> WhenDupeCheck(string WhenText)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("WhenText", WhenText);
            p.Add("WhenExists", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spWhenDupeCheck", p, "Default");

            var i = p.Get<int>("WhenExists");
            return Convert.ToBoolean(i);
        }
        public async Task<List<RandomRestaurant>> GetRandomRestaurant(int WhenID)
        {
            var recs = await _dataAccess.LoadData<RandomRestaurant, dynamic>(
                "scud97_kssu.spRandomRestaurant",
                new { WhenID },
                "Default");
            return recs;
        }
        public Task<int> RestaurantWatched(int RestaurantID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spRestaurantWatched",
                new { RestaurantID },
                "Default");
        }
        public Task<int> AddRestaurantFoodType(int RestaurantID, int FoodTypeID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spAddRestaurantFoodType",
                new { RestaurantID, FoodTypeID },
                "Default");
        }
        public Task<int> AddRestaurantWhen(int RestaurantID, int WhenID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spAddRestaurantWhen",
                new { RestaurantID, WhenID },
                "Default");
        }
        public async Task<List<RestaurantFoodTypes>> GetRestaurantFoodTypes(int RestaurantID)
        {
            var recs = await _dataAccess.LoadData<RestaurantFoodTypes, dynamic>(
                "scud97_kssu.spGetRestaurantFoodTypes",
                new { RestaurantID },
                "Default");
            return recs;
        }
        public async Task<List<RestaurantWhens>> GetRestaurantWhens(int RestaurantID)
        {
            var recs = await _dataAccess.LoadData<RestaurantWhens, dynamic>(
                "scud97_kssu.spGetRestaurantWhens",
                new { RestaurantID },
                "Default");
            return recs;
        }
        public Task<int> DeleteRestaurantFoodType(int RestaurantFoodTypeID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteRestaurantFoodType",
                new { RestaurantFoodTypeID },
                "Default");
        }
        public Task<int> DeleteRestaurantWhen(int RestaurantWhenID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteRestaurantWhen",
                new { RestaurantWhenID },
                "Default");
        }

    }
}
