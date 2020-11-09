using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static DatePot.Areas.FoodPot.Models.Foods;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DatePot.Areas.FoodPot.Data
{
    public class FoodData
    {
        public List<FilmDetails> GetFilmDetails(string cs, int FilmID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spGetFilmDetails"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmID", FilmID);
                IList<FilmDetails> wsl = new List<FilmDetails>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ws = new FilmDetails();
                    ws.FilmID = reader.GetInt32("FilmID");
                    ws.FilmName = reader.GetString("FilmName");
                    ws.ReleaseDate = reader.GetDateTime("ReleaseDate");
                    ws.GenreID = reader.GetInt32("GenreID");
                    ws.DirectorID = reader.GetInt32("DirectorID");
                    ws.Watched = reader.GetBoolean("Watched");
                    ws.AddedByID = reader.GetInt32("AddedByID");
                    wsl.Add(ws);
                }
                reader.Close();
                return (List<FilmDetails>)wsl;

            }
        }
        public List<Genres> GetGenreList(string cs)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetGenreList";
            using var cmd = new MySqlCommand(sql, con);
            IList<Genres> wsl = new List<Genres>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new Genres();
                ws.GenreID = reader.GetInt32("GenreID");
                ws.GenreText = reader.GetString("GenreText");
                wsl.Add(ws);
            }
            reader.Close();
            con.Close();
            return (List<Genres>)wsl;
        }
        public List<Directors> GetDirectorsList(string cs)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetDirectorsList";
            using var cmd = new MySqlCommand(sql, con);
            IList<Directors> wsl = new List<Directors>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new Directors();
                ws.DirectorID = reader.GetInt32("DirectorID");
                ws.DirectorName = reader.GetString("DirectorName");
                wsl.Add(ws);
            }
            reader.Close();
            con.Close();
            return (List<Directors>)wsl;
        }
        public void UpdateFilm(string cs, UpdateFilmDetails updatefiledetails)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spUpdateFilm"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmID", updatefiledetails.FilmID);
                cmd.Parameters.AddWithValue("@FilmName", updatefiledetails.FilmName);
                cmd.Parameters.AddWithValue("@ReleaseDate", updatefiledetails.ReleaseDate);
                cmd.Parameters.AddWithValue("@GenreID", updatefiledetails.GenreID);
                MySqlParameter paramWatched = new MySqlParameter
                {
                    ParameterName = "@Watched",
                    MySqlDbType = MySqlDbType.Bit,
                    Value = updatefiledetails.Watched
                };
                cmd.Parameters.Add(paramWatched);
                cmd.Parameters.AddWithValue("@AddedByID", updatefiledetails.AddedByID);
                cmd.Parameters.AddWithValue("@DirectorID", updatefiledetails.DirectorID);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
        public void ArchiveFilm(string cs, int FilmID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spArchiveFilm"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmID", FilmID);
                cmd.ExecuteNonQuery();

                con.Close();
            }
        }
        public List<FilmList> GetFilmList(string cs)
        {

            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetFilmList";
            using var cmd = new MySqlCommand(sql, con);
            IList<FilmList> wsl = new List<FilmList>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new FilmList();
                ws.FilmID = reader.GetInt32("FilmID");
                ws.FilmName = reader.GetString("FilmName");
                ws.ReleaseDate = reader.GetString("ReleaseDate");
                ws.AddedDate = reader.GetString("AddedDate");
                ws.GenreText = reader.GetString("GenreText");
                ws.Watched = reader.GetString("Watched");
                ws.UserName = reader.GetString("UserName");
                wsl.Add(ws);
            }
            reader.Close();
            return (List<FilmList>)wsl;
        }
        public List<UserList> GetUserList(string cs)
        {

            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spGetUserList";
            using var cmd = new MySqlCommand(sql, con);
            IList<UserList> wsl = new List<UserList>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new UserList();
                ws.UserID = reader.GetInt32("UserID");
                ws.UserName = reader.GetString("UserName");
                wsl.Add(ws);
            }
            reader.Close();
            return (List<UserList>)wsl;
        }

        public int AddFilm(string cs, NewFilm newfilm)
        {
            int AddedByID = 0;
            if (newfilm.AddersName == "Kyle")
            {
                AddedByID = 1;
            }
            else { AddedByID = 2; }
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddFilm"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmName", newfilm.FilmName);
                cmd.Parameters.AddWithValue("@ReleaseDate", newfilm.ReleaseDate);
                cmd.Parameters.AddWithValue("@AddedDate", newfilm.AddedDate);
                cmd.Parameters.AddWithValue("@GenreID", newfilm.GenreID);
                cmd.Parameters.AddWithValue("@Watched", newfilm.Watched);
                cmd.Parameters.AddWithValue("@AddedByID", AddedByID);
                cmd.Parameters.Add("@FilmID", MySqlDbType.Int32);
                cmd.Parameters["@FilmID"].Direction = ParameterDirection.Output; // from System.Data
                cmd.ExecuteNonQuery();
                Object obj = cmd.Parameters["@FilmID"].Value;
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
        public List<RandomFilm> GetRandomFilm(string cs)
        {

            using var con = new MySqlConnection(cs);
            con.Open();

            string sql = "CALL spRandomFilm";
            using var cmd = new MySqlCommand(sql, con);
            IList<RandomFilm> wsl = new List<RandomFilm>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var ws = new RandomFilm();
                ws.FilmID = reader.GetInt32("FilmID");
                ws.FilmName = reader.GetString("FilmName");
                ws.AddersName = reader.GetString("AddersName");
                wsl.Add(ws);
            }
            reader.Close();
            return (List<RandomFilm>)wsl;
        }
        public void FilmWatched(string cs, int FilmID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spFilmWatched"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmID", FilmID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
