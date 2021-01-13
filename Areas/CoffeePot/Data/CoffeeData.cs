using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static DatePot.Areas.CoffeePot.Models.Coffees;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DatePot.Areas.CoffeePot.Data
{
    public class CoffeeData
    {
        public List<CoffeeDetails> GetCoffeeDetails(string cs, int CoffeeID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spGetCoffeeDetails"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@CoffeeID", CoffeeID);
                IList<CoffeeDetails> wsl = new List<CoffeeDetails>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ws = new CoffeeDetails();
                    ws.CoffeeID = reader.GetInt32("CoffeeID");
                    ws.CoffeeName = reader.GetString("CoffeeName");
                    ws.KyleTaste = reader.GetInt32("KyleTaste");
                    ws.RhiannTaste = reader.GetInt32("RhiannTaste");
                    ws.KyleExperience = reader.GetInt32("KyleExperience");
                    ws.RhiannExperience = reader.GetInt32("RhiannExperience");
                    wsl.Add(ws);
                }
                reader.Close();
                return (List<CoffeeDetails>)wsl;

            }
        }
        public void UpdateCoffee(string cs, int CoffeeID, string CoffeeName, int KyleTaste, int RhiannTaste, int KyleExperience, int RhiannExperience)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spUpdateCoffee"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@CoffeeID", CoffeeID);
                cmd.Parameters.AddWithValue("@CoffeeName", CoffeeName);
                cmd.Parameters.AddWithValue("@KyleTaste", KyleTaste);
                cmd.Parameters.AddWithValue("@RhiannTaste", RhiannTaste);
                cmd.Parameters.AddWithValue("@KyleExperience", KyleExperience);
                cmd.Parameters.AddWithValue("@RhiannExperience", RhiannExperience);
                
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
        public void ArchiveCoffee(string cs, int CoffeeID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spArchiveCoffee"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@CoffeeID", CoffeeID);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
        public List<CoffeeList> GetCoffeeList(string cs)
        {

            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetCoffeeList";
            using var cmd = new MySqlCommand(sql, con);
            IList<CoffeeList> wsl = new List<CoffeeList>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new CoffeeList();
                ws.CoffeeID = reader.GetInt32("CoffeeID");
                ws.CoffeeName = reader.GetString("CoffeeName");
                ws.KyleTaste = reader.GetInt32("KyleTaste");
                ws.RhiannTaste = reader.GetInt32("RhiannTaste");
                ws.KyleExperience = reader.GetInt32("KyleExperience");
                ws.RhiannExperience = reader.GetInt32("RhiannExperience");
                ws.AvgRating = reader.GetInt32("AvgRating");
                wsl.Add(ws);
            }
            reader.Close();
            return (List<CoffeeList>)wsl;
        }
        public int AddCoffee(string cs, string CoffeeName)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddCoffee"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@CoffeeName", CoffeeName);
                cmd.Parameters.Add("@CoffeeID", MySqlDbType.Int32);
                cmd.Parameters["@CoffeeID"].Direction = ParameterDirection.Output; // from System.Data
                cmd.ExecuteNonQuery();
                Object obj = cmd.Parameters["@CoffeeID"].Value;
                var lParam = (Int32)obj;    // more useful datatype
                con.Close();
                return lParam;
            }
        }
        public List<RandomCoffee> GetRandomCoffee(string cs)
        {

            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spRandomCoffee";
            using var cmd = new MySqlCommand(sql, con);
            IList<RandomCoffee> wsl = new List<RandomCoffee>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new RandomCoffee();
                ws.CoffeeID = reader.GetInt32("CoffeeID");
                ws.CoffeeName = reader.GetString("CoffeeName");
                wsl.Add(ws);
            }
            reader.Close();
            return (List<RandomCoffee>)wsl;
        }
    }
}
