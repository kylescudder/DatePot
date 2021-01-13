using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static DatePot.Areas.ActivityPot.Models.Activitys;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DatePot.Areas.ActivityPot.Data
{
    public class ActivityData
    {
        public List<ActivityDetails> GetActivityDetails(string cs, int ActivityID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spGetActivityDetails"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                IList<ActivityDetails> wsl = new List<ActivityDetails>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ws = new ActivityDetails();
                    ws.ActivityID = reader.GetInt32("ActivityID");
                    ws.ActivityName = reader.GetString("ActivityName");
                    ws.Location = reader.GetString("Location");
                    ws.Description = reader.GetString("Description");
                    ws.ExpenseID = reader.GetInt32("ExpenseID");
                    ws.Prebook = reader.GetBoolean("Prebook");
                    wsl.Add(ws);
                }
                reader.Close();
                return (List<ActivityDetails>)wsl;

            }
        }
        public List<ActivityTypes> GetyActivityTypeList(string cs)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetActivityTypeList";
            using var cmd = new MySqlCommand(sql, con);
            IList<ActivityTypes> wsl = new List<ActivityTypes>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new ActivityTypes();
                ws.ActivityTypeID = reader.GetInt32("ActivityTypeID");
                ws.ActivityType = reader.GetString("ActivityType");
                wsl.Add(ws);
            }
            reader.Close();
            con.Close();
            return (List<ActivityTypes>)wsl;
        }
        public void UpdateActivity(string cs, int ActivityID, string ActivityName, string Location, int ExpenseID, string Description, bool Prebook)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spUpdateActivity"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                cmd.Parameters.AddWithValue("@ActivityName", ActivityName);
                cmd.Parameters.AddWithValue("@Location", Location);
                cmd.Parameters.AddWithValue("@ExpenseID", ExpenseID);
                cmd.Parameters.AddWithValue("@Description", Description);
                MySqlParameter paramWatched = new MySqlParameter
                {
                    ParameterName = "@Prebook",
                    MySqlDbType = MySqlDbType.Bit,
                    Value = Prebook
                };
                cmd.Parameters.Add(paramWatched);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
        public void ArchiveActivity(string cs, int ActivityID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spArchiveActivity"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
        public List<ActivityList> GetActivityList(string cs)
        {

            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetActivityList";
            using var cmd = new MySqlCommand(sql, con);
            IList<ActivityList> wsl = new List<ActivityList>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new ActivityList();
                ws.ActivityID = reader.GetInt32("ActivityID");
                ws.ActivityName = reader.GetString("ActivityName");
                ws.Location = reader.GetString("Location");
                ws.Description = reader.GetString("Description");
                ws.ExpenseText = reader.GetString("ExpenseText");
                ws.ActivityType = reader.GetString("ActivityType");
                ws.Prebook = reader.GetString("Prebook");
                wsl.Add(ws);
            }
            reader.Close();
            return (List<ActivityList>)wsl;
        }
        public int AddActivity(string cs, string ActivityName, string Location, int? ExpenseID, string Description, bool Prebook)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddActivity"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityName", ActivityName);
                cmd.Parameters.AddWithValue("@Location", Location);
                cmd.Parameters.AddWithValue("@Description", Description);
                cmd.Parameters.AddWithValue("@ExpenseID", ExpenseID);
                cmd.Parameters.AddWithValue("@Prebook", Prebook);
                cmd.Parameters.Add("@ActivityID", MySqlDbType.Int32);
                cmd.Parameters["@ActivityID"].Direction = ParameterDirection.Output; // from System.Data
                cmd.ExecuteNonQuery();
                Object obj = cmd.Parameters["@ActivityID"].Value;
                var lParam = (Int32)obj;    // more useful datatype
                con.Close();
                return lParam;
            }
        }
        public void AddGenre(string cs, string GenreText)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddGenre"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@GenreText", GenreText);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void AddDirector(string cs, string DirectorText)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddDirector"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@DirectorText", DirectorText);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public bool GenreDupeCheck(string cs, string GenreText)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spGenreDupeCheck"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@GenreText", GenreText);
                cmd.Parameters.Add("@GenreExists", MySqlDbType.Bool);
                cmd.Parameters["@GenreExists"].Direction = ParameterDirection.Output; // from System.Data
                cmd.ExecuteNonQuery();
                Object obj = cmd.Parameters["@GenreExists"].Value;
                var lParam = (Boolean)obj;    // more useful datatype

                cmd.ExecuteNonQuery();
                con.Close();
                return lParam;
            }
        }
        public bool DirectorDupeCheck(string cs, string DirectorText)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spDirectorDupeCheck"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@DirectorText", DirectorText);
                cmd.Parameters.Add("@DirectorExists", MySqlDbType.Bool);
                cmd.Parameters["@DirectorExists"].Direction = ParameterDirection.Output; // from System.Data
                cmd.ExecuteNonQuery();
                Object obj = cmd.Parameters["@DirectorExists"].Value;
                var lParam = (Boolean)obj;    // more useful datatype

                cmd.ExecuteNonQuery();
                con.Close();
                return lParam;
            }
        }
        public List<RandomActivity> GetRandomActivity(string cs)
        {

            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spRandomActivity";
            using var cmd = new MySqlCommand(sql, con);
            IList<RandomActivity> wsl = new List<RandomActivity>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new RandomActivity();
                ws.ActivityID = reader.GetInt32("ActivityID");
                ws.ActivityName = reader.GetString("ActivityName");
                ws.Location = reader.GetString("Location");
                wsl.Add(ws);
            }
            reader.Close();
            return (List<RandomActivity>)wsl;
        }
        public void ActivityWatched(string cs, int ActivityID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spActivityWatched"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public List<Expenses> GetExpenseList(string cs)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetExpenseList";
            using var cmd = new MySqlCommand(sql, con);
            IList<Expenses> wsl = new List<Expenses>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new Expenses();
                ws.ExpenseID = reader.GetInt32("ExpenseID");
                ws.ExpenseText = reader.GetString("ExpenseText");
                wsl.Add(ws);
            }
            reader.Close();
            con.Close();
            return (List<Expenses>)wsl;
        }
        public bool ActivityTypeDupeCheck(string cs, string ActivityTypeText)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spActivityTypeDupeCheck"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityTypeText", ActivityTypeText);
                cmd.Parameters.Add("@ActivityTypeExists", MySqlDbType.Bool);
                cmd.Parameters["@ActivityTypeExists"].Direction = ParameterDirection.Output; // from System.Data
                cmd.ExecuteNonQuery();
                Object obj = cmd.Parameters["@ActivityTypeExists"].Value;
                var lParam = (Boolean)obj;    // more useful datatype

                cmd.ExecuteNonQuery();
                con.Close();
                return lParam;
            }
        }
        public void AddActivityType(string cs, string ActivityTypeText)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddActivityType"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityTypeText", ActivityTypeText);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void AddActivityxType(string cs, int ActivityID, int ActivityTypeID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddActivityxType"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                cmd.Parameters.AddWithValue("@ActivityTypeID", ActivityTypeID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public List<ActivityxTypes> GetActivityxTypes(string cs, int ActivityID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spGetActivityxTypes"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                IList<ActivityxTypes> wsl = new List<ActivityxTypes>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ws = new ActivityxTypes();
                    ws.ActivityXTypeID = reader.GetInt32("ActivityxTypeID");
                    ws.ActivityTypeText = reader.GetString("ActivityTypeText");

                    wsl.Add(ws);
                }
                reader.Close();
                return (List<ActivityxTypes>)wsl;

            }
        }
        public void DeleteActivityxType(string cs, int ActivityxTypeID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spDeleteActivityxType"; 
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityxTypesID", ActivityxTypeID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

    }
}
