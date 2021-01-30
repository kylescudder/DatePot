using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using static DatePot.Areas.FoodPot.Models.Food;
using Microsoft.Extensions.Configuration;
using System.Data;
using DatePot.Db;
using Dapper;

namespace DatePot.Areas.FoodPot.Data
{
    public class FoodData : IFoodData
    {
        private readonly ISqlDb _dataAccess;
        public FoodData(ISqlDb dataAccess
            //, IHttpContextAccessor httpContextAccessor
            )
        {
            _dataAccess = dataAccess;
            //_httpContextAccessor = httpContextAccessor;
        }
        public List<RandomRestaurant> RandomRestaurants { get; set; }
        public async Task<List<RestaurantDetails>> GetRestaurantDetails(int RestaurantID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetRestaurantDetails"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
            //    IList<RestaurantDetails> wsl = new List<RestaurantDetails>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new RestaurantDetails();
            //        ws.RestaurantID = reader.GetInt32("RestaurantID");
            //        ws.RestaurantName = reader.GetString("RestaurantName");
            //        ws.ExpenseID = reader.GetInt16("ExpenseID");
            //        ws.LocationID = reader.GetInt16("LocationID");
            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<RestaurantDetails>)wsl;

            //}
            var recs = await _dataAccess.LoadData<RestaurantDetails, dynamic>(
                "scud97_kssu.spGetRestaurantDetails",
                new { RestaurantID },
                "Default");
            return recs;
        }
        public async Task<List<FoodTypes>> GetFoodTypeList()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetFoodTypeList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<FoodTypes> wsl = new List<FoodTypes>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new FoodTypes();
            //    ws.FoodTypeID = reader.GetInt32("FoodTypeID");
            //    ws.FoodTypeText = reader.GetString("FoodTypeText");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //con.Close();
            //return (List<FoodTypes>)wsl;
            var recs = await _dataAccess.LoadData<FoodTypes, dynamic>(
                "scud97_kssu.spGetFoodTypeList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<When>> GetWhenList()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetWhenList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<When> wsl = new List<When>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new When();
            //    ws.WhenID = reader.GetInt32("WhenID");
            //    ws.WhenText = reader.GetString("WhenText");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //con.Close();
            //return (List<When>)wsl;
            var recs = await _dataAccess.LoadData<When, dynamic>(
                "scud97_kssu.spGetWhenList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<Expenses>> GetExpenseList()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetExpenseList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<Expenses> wsl = new List<Expenses>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new Expenses();
            //    ws.ExpenseID = reader.GetInt32("ExpenseID");
            //    ws.ExpenseText = reader.GetString("ExpenseText");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //con.Close();
            //return (List<Expenses>)wsl;
            var recs = await _dataAccess.LoadData<Expenses, dynamic>(
                "scud97_kssu.spGetExpenseList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<Locations>> GetLocationList()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetLocationList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<Locations> wsl = new List<Locations>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new Locations();
            //    ws.LocationID = reader.GetInt32("LocationID");
            //    ws.LocationText = reader.GetString("LocationText");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //con.Close();
            //return (List<Locations>)wsl;
            var recs = await _dataAccess.LoadData<Locations, dynamic>(
                "scud97_kssu.spGetLocationList",
                new { },
                "Default");
            return recs;
        }

        public Task<int> UpdateRestaurant(int RestaurantID, string RestaurantName, int ExpenseID, int LocationID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spUpdateRestaurant"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
            //    cmd.Parameters.AddWithValue("@RestaurantName", RestaurantName);
            //    cmd.Parameters.AddWithValue("@ExpenseID", ExpenseID);
            //    cmd.Parameters.AddWithValue("@LocationID", LocationID);
            //    cmd.ExecuteNonQuery();

            //    con.Close();
            //}
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
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spArchiveRestaurant"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
            //    cmd.ExecuteNonQuery();

            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spArchiveRestaurant",
                new { RestaurantID },
                "Default");
        }
        public async Task<List<RestaurantList>> GetRestaurantList()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetRestaurantList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<RestaurantList> wsl = new List<RestaurantList>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new RestaurantList();
            //    ws.RestaurantID = reader.GetInt32("RestaurantID");
            //    ws.RestaurantName = reader.GetString("RestaurantName");
            //    ws.ExpenseText = reader.GetString("ExpenseText");
            //    ws.LocationText = reader.GetString("LocationText");
            //    ws.FoodTypeText = reader.GetString("FoodTypeText");
            //    ws.WhenText = reader.GetString("WhenText");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //return (List<RestaurantList>)wsl;
            var recs = await _dataAccess.LoadData<RestaurantList, dynamic>(
                "scud97_kssu.spGetRestaurantList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<UserList>> GetUserList()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetUserList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<UserList> wsl = new List<UserList>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new UserList();
            //    ws.UserID = reader.GetInt32("UserID");
            //    ws.UserName = reader.GetString("UserName");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //return (List<UserList>)wsl;
            var recs = await _dataAccess.LoadData<UserList, dynamic>(
                "scud97_kssu.spGetUserList",
                new { },
                "Default");
            return recs;
        }

        public async Task<int> AddRestaurant(string RestaurantName, int ExpenseID, int LocationID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddRestaurant"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantName", RestaurantName);
            //    cmd.Parameters.AddWithValue("@ExpenseID", ExpenseID);
            //    cmd.Parameters.AddWithValue("@LocationID", LocationID);
            //    cmd.Parameters.Add("@RestaurantID", MySqlDbType.Int32);
            //    cmd.Parameters["@RestaurantID"].Direction = ParameterDirection.Output; // from System.Data
            //    cmd.ExecuteNonQuery();
            //    Object obj = cmd.Parameters["@RestaurantID"].Value;
            //    var lParam = (Int32)obj;    // more useful datatype
            //    con.Close();
            //    return lParam;
            //}
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
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddFoodType"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FoodTypeText", FoodTypeText);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddFoodType",
                new { FoodTypeText },
                "Default");
        }
        public Task<int> AddWhen(string WhenText)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddWhen"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@WhenText", WhenText);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddWhen",
                new { WhenText },
                "Default");
        }
        public async Task<bool> FoodTypeDupeCheck(string FoodTypeText)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spFoodTypeDupeCheck"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FoodTypeText", FoodTypeText);
            //    cmd.Parameters.Add("@FoodTypeExists", MySqlDbType.Bool);
            //    cmd.Parameters["@FoodTypeExists"].Direction = ParameterDirection.Output; // from System.Data
            //    cmd.ExecuteNonQuery();
            //    Object obj = cmd.Parameters["@FoodTypeExists"].Value;
            //    var lParam = (Boolean)obj;    // more useful datatype

            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //    return lParam;
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("FoodTypeText", FoodTypeText);
            p.Add("FoodTypeExists", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spFoodTypeDupeCheck", p, "Default");

            var i = p.Get<int>("FoodTypeExists");
            return Convert.ToBoolean(i);
        }
        public async Task<bool> WhenDupeCheck(string WhenText)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spWhenDupeCheck"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@WhenText", WhenText);
            //    cmd.Parameters.Add("@WhenExists", MySqlDbType.Bool);
            //    cmd.Parameters["@WhenExists"].Direction = ParameterDirection.Output; // from System.Data
            //    cmd.ExecuteNonQuery();
            //    Object obj = cmd.Parameters["@WhenExists"].Value;
            //    var lParam = (Boolean)obj;    // more useful datatype

            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //    return lParam;
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("WhenText", WhenText);
            p.Add("WhenExists", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spWhenDupeCheck", p, "Default");

            var i = p.Get<int>("WhenExists");
            return Convert.ToBoolean(i);
        }
        public async Task<List<RandomRestaurant>> GetRandomRestaurant(int WhenID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spRandomRestaurant (" + WhenID + ")";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<RandomRestaurant> wsl = new List<RandomRestaurant>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new RandomRestaurant();
            //    ws.RestaurantID = reader.GetInt32("RestaurantID");
            //    ws.RestaurantName = reader.GetString("RestaurantName");
            //    ws.ExpenseText = reader.GetString("ExpenseText");
            //    ws.LocationText = reader.GetString("LocationText");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //RandomRestaurants = (List<RandomRestaurant>)wsl;
            //return (List<RandomRestaurant>)wsl;
            var recs = await _dataAccess.LoadData<RandomRestaurant, dynamic>(
                "scud97_kssu.spRandomRestaurant",
                new { WhenID },
                "Default");
            return recs;
        }
        public Task<int> RestaurantWatched(int RestaurantID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spRestaurantWatched"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spRestaurantWatched",
                new { RestaurantID },
                "Default");
        }
        public Task<int> AddRestaurantFoodType(int RestaurantID, int FoodTypeID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddRestaurantFoodType"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
            //    cmd.Parameters.AddWithValue("@FoodTypeID", FoodTypeID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddRestaurantFoodType",
                new { RestaurantID, FoodTypeID },
                "Default");
        }
        public Task<int> AddRestaurantWhen(int RestaurantID, int WhenID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddRestaurantWhen"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
            //    cmd.Parameters.AddWithValue("@WhenID", WhenID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddRestaurantWhen",
                new { RestaurantID, WhenID },
                "Default");
        }
        public async Task<List<RestaurantFoodTypes>> GetRestaurantFoodTypes(int RestaurantID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetRestaurantFoodTypes"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
            //    IList<RestaurantFoodTypes> wsl = new List<RestaurantFoodTypes>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new RestaurantFoodTypes();
            //        ws.RestaurantFoodTypeID = reader.GetInt32("RestaurantFoodTypeID");
            //        ws.FoodTypeText = reader.GetString("FoodType");

            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<RestaurantFoodTypes>)wsl;

            //}
            var recs = await _dataAccess.LoadData<RestaurantFoodTypes, dynamic>(
                "scud97_kssu.spGetRestaurantFoodTypes",
                new { RestaurantID },
                "Default");
            return recs;
        }
        public async Task<List<RestaurantWhens>> GetRestaurantWhens(int RestaurantID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetRestaurantWhens"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantID", RestaurantID);
            //    IList<RestaurantWhens> wsl = new List<RestaurantWhens>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new RestaurantWhens();
            //        ws.RestaurantWhenID = reader.GetInt32("RestaurantsWhensID");
            //        ws.WhenText = reader.GetString("WhenText");

            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<RestaurantWhens>)wsl;
            //}
            var recs = await _dataAccess.LoadData<RestaurantWhens, dynamic>(
                "scud97_kssu.spGetRestaurantWhens",
                new { RestaurantID },
                "Default");
            return recs;
        }
        public Task<int> DeleteRestaurantFoodType(int RestaurantFoodTypeID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spDeleteRestaurantFoodType"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantFoodTypesID", RestaurantFoodTypeID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteRestaurantFoodType",
                new { RestaurantFoodTypeID },
                "Default");
        }
        public Task<int> DeleteRestaurantWhen(int RestaurantWhenID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spDeleteRestaurantWhen"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@RestaurantWhensID", RestaurantWhenID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteRestaurantWhen",
                new { RestaurantWhenID },
                "Default");
        }

    }
}
