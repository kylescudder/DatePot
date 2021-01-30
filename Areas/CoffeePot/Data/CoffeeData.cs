using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using static DatePot.Areas.CoffeePot.Models.Coffees;
using Microsoft.Extensions.Configuration;
using System.Data;
using DatePot.Db;
using Dapper;

namespace DatePot.Areas.CoffeePot.Data
{
    public class CoffeeData : ICoffeeData
    {
        private readonly ISqlDb _dataAccess;
        public CoffeeData(ISqlDb dataAccess
            //, IHttpContextAccessor httpContextAccessor
            )
        {
            _dataAccess = dataAccess;
            //_httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<CoffeeDetails>> GetCoffeeDetails(int CoffeeID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetCoffeeDetails"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@CoffeeID", CoffeeID);
            //    IList<CoffeeDetails> wsl = new List<CoffeeDetails>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new CoffeeDetails();
            //        ws.CoffeeID = reader.GetInt32("CoffeeID");
            //        ws.CoffeeName = reader.GetString("CoffeeName");
            //        ws.KyleTaste = reader.GetInt32("KyleTaste");
            //        ws.RhiannTaste = reader.GetInt32("RhiannTaste");
            //        ws.KyleExperience = reader.GetInt32("KyleExperience");
            //        ws.RhiannExperience = reader.GetInt32("RhiannExperience");
            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<CoffeeDetails>)wsl;
            //}
            var recs = await _dataAccess.LoadData<CoffeeDetails, dynamic>(
                "scud97_kssu.spGetCoffeeDetails",
                new { CoffeeID },
                "Default");
            return recs;
        }
        public Task<int> UpdateCoffee(int CoffeeID, string CoffeeName, int KyleTaste, int RhiannTaste, int KyleExperience, int RhiannExperience)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spUpdateCoffee"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@CoffeeID", CoffeeID);
            //    cmd.Parameters.AddWithValue("@CoffeeName", CoffeeName);
            //    cmd.Parameters.AddWithValue("@KyleTaste", KyleTaste);
            //    cmd.Parameters.AddWithValue("@RhiannTaste", RhiannTaste);
            //    cmd.Parameters.AddWithValue("@KyleExperience", KyleExperience);
            //    cmd.Parameters.AddWithValue("@RhiannExperience", RhiannExperience);

            //    cmd.ExecuteNonQuery();

            //    con.Close();
            //}
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
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spArchiveCoffee"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@CoffeeID", CoffeeID);
            //    cmd.ExecuteNonQuery();

            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spArchiveCoffee",
                new { CoffeeID },
                "Default");
        }
        public async Task<List<CoffeeList>> GetCoffeeList()
        {

            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetCoffeeList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<CoffeeList> wsl = new List<CoffeeList>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new CoffeeList();
            //    ws.CoffeeID = reader.GetInt32("CoffeeID");
            //    ws.CoffeeName = reader.GetString("CoffeeName");
            //    ws.KyleTaste = reader.GetInt32("KyleTaste");
            //    ws.RhiannTaste = reader.GetInt32("RhiannTaste");
            //    ws.KyleExperience = reader.GetInt32("KyleExperience");
            //    ws.RhiannExperience = reader.GetInt32("RhiannExperience");
            //    ws.AvgRating = reader.GetInt32("AvgRating");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //return (List<CoffeeList>)wsl;
            var recs = await _dataAccess.LoadData<CoffeeList, dynamic>(
                "scud97_kssu.spGetCoffeeList",
                new { },
                "Default");
            return recs;
        }
        public async Task<int> AddCoffee(string CoffeeName)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddCoffee"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@CoffeeName", CoffeeName);
            //    cmd.Parameters.Add("@CoffeeID", MySqlDbType.Int32);
            //    cmd.Parameters["@CoffeeID"].Direction = ParameterDirection.Output; // from System.Data
            //    cmd.ExecuteNonQuery();
            //    Object obj = cmd.Parameters["@CoffeeID"].Value;
            //    var lParam = (Int32)obj;    // more useful datatype
            //    con.Close();
            //    return lParam;
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("CoffeeName", CoffeeName);
            p.Add("CoffeeID", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spAddCoffee", p, "Default");

            return p.Get<int>("CoffeeID");
        }
        public async Task<List<RandomCoffee>> GetRandomCoffee()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spRandomCoffee";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<RandomCoffee> wsl = new List<RandomCoffee>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new RandomCoffee();
            //    ws.CoffeeID = reader.GetInt32("CoffeeID");
            //    ws.CoffeeName = reader.GetString("CoffeeName");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //return (List<RandomCoffee>)wsl;
            var recs = await _dataAccess.LoadData<RandomCoffee, dynamic>(
                "scud97_kssu.spRandomCoffee",
                new { },
                "Default");
            return recs;
        }
    }
}
