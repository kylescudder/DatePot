using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static DatePot.Areas.ActivityPot.Models.Activitys;
using Microsoft.Extensions.Configuration;
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
        public async Task<List<ActivityDetails>> GetActivityDetails(int ActivityID)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetActivityDetails"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
            //    IList<ActivityDetails> wsl = new List<ActivityDetails>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new ActivityDetails();
            //        ws.ActivityID = reader.GetInt32("ActivityID");
            //        ws.ActivityName = reader.GetString("ActivityName");
            //        ws.Location = reader.GetString("Location");
            //        ws.Description = reader.GetString("Description");
            //        ws.ExpenseID = reader.GetInt32("ExpenseID");
            //        ws.Prebook = reader.GetBoolean("Prebook");
            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<ActivityDetails>)wsl;
            //}
            var recs = await _dataAccess.LoadData<ActivityDetails, dynamic>(
                "scud97_kssu.spGetActivityDetails",
                new { ActivityID },
                "Default");
            return recs;
        }
        public async Task<List<ActivityTypes>> GetyActivityTypeList()
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //string sql = "CALL spGetActivityTypeList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<ActivityTypes> wsl = new List<ActivityTypes>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new ActivityTypes();
            //    ws.ActivityTypeID = reader.GetInt32("ActivityTypeID");
            //    ws.ActivityType = reader.GetString("ActivityType");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //con.Close();
            //return (List<ActivityTypes>)wsl;
            var recs = await _dataAccess.LoadData<ActivityTypes, dynamic>(
                "scud97_kssu.spGetActivityTypeList",
                new { },
                "Default");
            return recs;
        }
        public Task<int> UpdateActivity(int ActivityID, string ActivityName, string Location, int ExpenseID, string Description, bool Prebook)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spUpdateActivity"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
            //    cmd.Parameters.AddWithValue("@ActivityName", ActivityName);
            //    cmd.Parameters.AddWithValue("@Location", Location);
            //    cmd.Parameters.AddWithValue("@ExpenseID", ExpenseID);
            //    cmd.Parameters.AddWithValue("@Description", Description);
            //    MySqlParameter paramWatched = new MySqlParameter
            //    {
            //        ParameterName = "@Prebook",
            //        MySqlDbType = MySqlDbType.Bit,
            //        Value = Prebook
            //    };
            //    cmd.Parameters.Add(paramWatched);
            //    cmd.ExecuteNonQuery();

            //    con.Close();
            //}
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
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spArchiveActivity"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
            //    cmd.ExecuteNonQuery();

            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spArchiveActivity",
                new { ActivityID },
                "Default");
        }
        public async Task<List<ActivityList>> GetActivityList()
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //string sql = "CALL spGetActivityList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<ActivityList> wsl = new List<ActivityList>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new ActivityList();
            //    ws.ActivityID = reader.GetInt32("ActivityID");
            //    ws.ActivityName = reader.GetString("ActivityName");
            //    ws.Location = reader.GetString("Location");
            //    ws.Description = reader.GetString("Description");
            //    ws.ExpenseText = reader.GetString("ExpenseText");
            //    ws.ActivityType = reader.GetString("ActivityType");
            //    ws.Prebook = reader.GetString("Prebook");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //return (List<ActivityList>)wsl;
            var recs = await _dataAccess.LoadData<ActivityList, dynamic>(
                "scud97_kssu.spGetActivityList",
                new { },
                "Default");
            return recs;
        }
        public async Task<int> AddActivity(string ActivityName, string Location, int? ExpenseID, string Description, bool Prebook)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddActivity"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@ActivityName", ActivityName);
            //    cmd.Parameters.AddWithValue("@Location", Location);
            //    cmd.Parameters.AddWithValue("@Description", Description);
            //    cmd.Parameters.AddWithValue("@ExpenseID", ExpenseID);
            //    cmd.Parameters.AddWithValue("@Prebook", Prebook);
            //    cmd.Parameters.Add("@ActivityID", MySqlDbType.Int32);
            //    cmd.Parameters["@ActivityID"].Direction = ParameterDirection.Output; // from System.Data
            //    cmd.ExecuteNonQuery();
            //    Object obj = cmd.Parameters["@ActivityID"].Value;
            //    var lParam = (Int32)obj;    // more useful datatype
            //    con.Close();
            //    return lParam;
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("ActivityName", ActivityName);
            p.Add("Location", Location);
            p.Add("Description", Description);
            p.Add("ExpenseID", ExpenseID);
            p.Add("Prebook", Prebook);
            p.Add("ActivityID", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spAddActivity", p, "Default");

            return p.Get<int>("ActivityID");
        }
        public Task<int> AddGenre(string GenreText)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddGenre"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@GenreText", GenreText);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddGenre",
                new { GenreText },
                "Default");
        }
        public Task<int> AddDirector(string DirectorText)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddDirector"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@DirectorText", DirectorText);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddDirector",
                new { DirectorText },
                "Default");
        }
        public async Task<List<RandomActivity>> GetRandomActivity()
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //string sql = "CALL spRandomActivity";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<RandomActivity> wsl = new List<RandomActivity>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new RandomActivity();
            //    ws.ActivityID = reader.GetInt32("ActivityID");
            //    ws.ActivityName = reader.GetString("ActivityName");
            //    ws.Location = reader.GetString("Location");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //return (List<RandomActivity>)wsl;
            var recs = await _dataAccess.LoadData<RandomActivity, dynamic>(
                "scud97_kssu.spRandomActivity",
                new { },
                "Default");
            return recs;
        }
        public Task<int> ActivityWatched(int ActivityID)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spActivityWatched"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spActivityWatched",
                new { ActivityID },
                "Default");
        }
        public async Task<List<Expenses>> GetExpenseList()
        {
            //using var con = new MySqlConnection();
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
        public async Task<bool> ActivityTypeDupeCheck(string ActivityTypeText)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spActivityTypeDupeCheck"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@ActivityTypeText", ActivityTypeText);
            //    cmd.Parameters.Add("@ActivityTypeExists", MySqlDbType.Bool);
            //    cmd.Parameters["@ActivityTypeExists"].Direction = ParameterDirection.Output; // from System.Data
            //    cmd.ExecuteNonQuery();
            //    Object obj = cmd.Parameters["@ActivityTypeExists"].Value;
            //    var lParam = (Boolean)obj;    // more useful datatype

            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //    return lParam;
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("ActivityTypeText", ActivityTypeText);
            p.Add("ActivityTypeExists", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spActivityTypeDupeCheck", p, "Default");

            var i = p.Get<int>("ActivityTypeExists");
            return Convert.ToBoolean(i);
        }
        public Task<int> AddActivityType(string ActivityTypeText)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddActivityType"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@ActivityTypeText", ActivityTypeText);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddActivityType",
                new { ActivityTypeText },
                "Default");
        }
        public Task<int> AddActivityxType(int ActivityID, int ActivityTypeID)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddActivityxType"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
            //    cmd.Parameters.AddWithValue("@ActivityTypeID", ActivityTypeID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddActivityxType",
                new { ActivityID, ActivityTypeID },
                "Default");
        }
        public async Task<List<ActivityxTypes>> GetActivityxTypes(int ActivityID)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetActivityxTypes"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
            //    IList<ActivityxTypes> wsl = new List<ActivityxTypes>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new ActivityxTypes();
            //        ws.ActivityXTypeID = reader.GetInt32("ActivityxTypeID");
            //        ws.ActivityTypeText = reader.GetString("ActivityTypeText");

            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<ActivityxTypes>)wsl;
            //}
            var recs = await _dataAccess.LoadData<ActivityxTypes, dynamic>(
                "scud97_kssu.spGetActivityxTypes",
                new { ActivityID },
                "Default");
            return recs;
        }
        public Task<int> DeleteActivityxType(int ActivityxTypeID)
        {
            //using var con = new MySqlConnection();
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spDeleteActivityxType"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@ActivityxTypesID", ActivityxTypeID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteActivityxType",
                new { ActivityxTypeID },
                "Default");
        }

    }
}
