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
                cmd.CommandText = "spGetActivityDetails"; // The name of the Stored Proc
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
                    ws.ActivityTypeID = reader.GetInt32("ActivityTypeID");
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
        public void UpdateActivity(string cs, UpdateActivityDetails updatefiledetails)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spUpdateActivity"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityID", updatefiledetails.ActivityID);
                cmd.Parameters.AddWithValue("@ActivityName", updatefiledetails.ActivityName);
                cmd.Parameters.AddWithValue("@Location", updatefiledetails.Location);
                cmd.Parameters.AddWithValue("@Description", updatefiledetails.Description);
                MySqlParameter paramWatched = new MySqlParameter
                {
                    ParameterName = "@Watched",
                    MySqlDbType = MySqlDbType.Bit,
                    Value = updatefiledetails.Prebook
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
                cmd.CommandText = "spArchiveActivity"; // The name of the Stored Proc
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
        public int AddActivity(string cs, NewActivity newActivity)
        {
            int AddedByID = 0;
            if (newActivity.AddersName == "Kyle")
            {
                AddedByID = 1;
            }
            else { AddedByID = 2; }
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddActivity"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityName", newActivity.ActivityName);
                cmd.Parameters.AddWithValue("@Location", newActivity.Location);
                cmd.Parameters.AddWithValue("@Description", newActivity.Description);
                cmd.Parameters.AddWithValue("@ExpenseID", newActivity.ExpenseID);
                cmd.Parameters.AddWithValue("@Prebook", newActivity.Prebook);
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
                cmd.CommandText = "spAddGenre"; // The name of the Stored Proc
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
                cmd.CommandText = "spAddDirector"; // The name of the Stored Proc
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
                cmd.CommandText = "spGenreDupeCheck"; // The name of the Stored Proc
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
                cmd.CommandText = "spDirectorDupeCheck"; // The name of the Stored Proc
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
                cmd.CommandText = "spActivityWatched"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@ActivityID", ActivityID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
