using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static DatePot.Areas.FilmPot.Models.Films;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace DatePot.Areas.FilmPot.Data
{
    public class FilmData
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
                    ws.AddedDate = reader.GetDateTime("AddedDate");
                    ws.Watched = reader.GetBoolean("Watched");
                    ws.AddedByID = reader.GetInt32("AddedByID");
                    ws.Runtime = reader.GetInt32("Runtime");
                    wsl.Add(ws);
                }
                reader.Close();
                return (List<FilmDetails>)wsl;

            }
        }
        public List<FilmGenres> GetFilmGenres(string cs, int FilmID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spGetFilmGenres"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmID", FilmID);
                IList<FilmGenres> wsl = new List<FilmGenres>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ws = new FilmGenres();
                    ws.FilmGenreID = reader.GetInt32("FilmGenreID");
                    ws.GenreText = reader.GetString("GenreText");

                    wsl.Add(ws);
                }
                reader.Close();
                return (List<FilmGenres>)wsl;

            }
        }
        public List<FilmDirectors> GetFilmDirectors(string cs, int FilmID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spGetFilmDirectors"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmID", FilmID);
                IList<FilmDirectors> wsl = new List<FilmDirectors>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var ws = new FilmDirectors();
                    ws.FilmDirectorID = reader.GetInt32("FilmDirectorID");
                    ws.DirectorText = reader.GetString("DirectorText");

                    wsl.Add(ws);
                }
                reader.Close();
                return (List<FilmDirectors>)wsl;

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
        public void UpdateFilm(string cs, int FilmID, int AddedByID, string FilmName, DateTime ReleaseDate, DateTime AddedDate, bool Watched, int Runtime)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spUpdateFilm"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmID", FilmID);
                cmd.Parameters.AddWithValue("@FilmName", FilmName);
                cmd.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
                cmd.Parameters.AddWithValue("@AddedDate", AddedDate);
                MySqlParameter paramWatched = new MySqlParameter
                {
                    ParameterName = "@Watched",
                    MySqlDbType = MySqlDbType.Bit,
                    Value = Watched
                };
                cmd.Parameters.Add(paramWatched);
                cmd.Parameters.AddWithValue("@AddedByID", AddedByID);
                cmd.Parameters.AddWithValue("@Runtime", Runtime);
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
                ws.Runtime = reader.GetInt32("Runtime");
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

        public int AddFilm(string cs, string AddersName, DateTime AddedDate, string FilmName, DateTime ReleaseDate, bool Watched, int Runtime)
        {
            int AddedByID = 0;
            if (AddersName == "Kyle")
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

                cmd.Parameters.AddWithValue("@FilmName", FilmName);
                cmd.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
                cmd.Parameters.AddWithValue("@AddedDate", AddedDate);
                cmd.Parameters.AddWithValue("@Watched", Watched);
                cmd.Parameters.AddWithValue("@AddedByID", AddedByID);
                cmd.Parameters.AddWithValue("@Runtime", Runtime);
                cmd.Parameters.Add("@FilmID", MySqlDbType.Int32);
                cmd.Parameters["@FilmID"].Direction = ParameterDirection.Output; // from System.Data
                cmd.ExecuteNonQuery();
                Object obj = cmd.Parameters["@FilmID"].Value;
                var lParam = (Int32)obj;    // more useful datatype
                con.Close();
                return lParam;
            }
        }
        public void AddFilmGenres(string cs, int FilmID, int GenreID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddFilmGenres"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmID", FilmID);
                cmd.Parameters.AddWithValue("@GenreID", GenreID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void AddFilmDirectors(string cs, int FilmID, int DirectorID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spAddFilmDirectors"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmID", FilmID);
                cmd.Parameters.AddWithValue("@DirectorID", DirectorID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void DeleteFilmGenre(string cs, int FilmGenreID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spDeleteFilmGenre"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmGenresID", FilmGenreID);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void DeleteFilmDirector(string cs, int FilmDirectorID)
        {
            using var con = new MySqlConnection(cs);
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = "spDeleteFilmDirector"; // The name of the Stored Proc
                cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

                cmd.Parameters.AddWithValue("@FilmDirectorsID", FilmDirectorID);
                cmd.ExecuteNonQuery();
                con.Close();
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
