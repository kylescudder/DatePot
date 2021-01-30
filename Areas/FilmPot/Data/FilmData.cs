using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using static DatePot.Areas.FilmPot.Models.Films;
using Microsoft.Extensions.Configuration;
using System.Data;
using DatePot.Db;
using Dapper;

namespace DatePot.Areas.FilmPot.Data
{
    public class FilmData : IFilmData
    {
        private readonly ISqlDb _dataAccess;
        //private readonly IHttpContextAccessor _httpContextAccessor;
        public FilmData(ISqlDb dataAccess
            //, IHttpContextAccessor httpContextAccessor
            )
        {
            _dataAccess = dataAccess;
            //_httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<FilmDetails>> GetFilmDetails(int FilmID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetFilmDetails"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmID", FilmID);
            //    IList<FilmDetails> wsl = new List<FilmDetails>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new FilmDetails();
            //        ws.FilmID = reader.GetInt32("FilmID");
            //        ws.FilmName = reader.GetString("FilmName");
            //        ws.ReleaseDate = reader.GetDateTime("ReleaseDate");
            //        ws.AddedDate = reader.GetDateTime("AddedDate");
            //        ws.Watched = reader.GetBoolean("Watched");
            //        ws.AddedByID = reader.GetInt32("AddedByID");
            //        ws.Runtime = reader.GetInt32("Runtime");
            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<FilmDetails>)wsl;

            //}
            var recs = await _dataAccess.LoadData<FilmDetails, dynamic>(
                "scud97_kssu.spGetFilmDetails",
                new { FilmID },
                "Default");
            return recs;

        }
        public async Task<List<FilmGenres>> GetFilmGenres(int FilmID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetFilmGenres"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmID", FilmID);
            //    IList<FilmGenres> wsl = new List<FilmGenres>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new FilmGenres();
            //        ws.FilmGenreID = reader.GetInt32("FilmGenreID");
            //        ws.GenreText = reader.GetString("GenreText");

            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<FilmGenres>)wsl;

            //}

            var recs = await _dataAccess.LoadData<FilmGenres, dynamic>(
                "scud97_kssu.spGetFilmGenres",
                new { FilmID },
                "Default");
            return recs;

        }
        public async Task<List<FilmDirectors>> GetFilmDirectors(int FilmID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetFilmDirectors"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmID", FilmID);
            //    IList<FilmDirectors> wsl = new List<FilmDirectors>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new FilmDirectors();
            //        ws.FilmDirectorID = reader.GetInt32("FilmDirectorID");
            //        ws.DirectorText = reader.GetString("DirectorText");

            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<FilmDirectors>)wsl;

            //}
            var recs = await _dataAccess.LoadData<FilmDirectors, dynamic>(
                "scud97_kssu.spGetFilmDirectors",
                new { FilmID },
                "Default");
            return recs;

        }
        public async Task<List<FilmPlatforms>> GetFilmPlatforms(int FilmID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGetFilmPlatforms"; 
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmID", FilmID);
            //    IList<FilmPlatforms> wsl = new List<FilmPlatforms>();
            //    var reader = cmd.ExecuteReader();
            //    while (reader.Read())
            //    {
            //        var ws = new FilmPlatforms();
            //        ws.FilmPlatformID = reader.GetInt32("FilmPlatformID");
            //        ws.PlatformText = reader.GetString("PlatformText");

            //        wsl.Add(ws);
            //    }
            //    reader.Close();
            //    return (List<FilmPlatforms>)wsl;

            //}
            var recs = await _dataAccess.LoadData<FilmPlatforms, dynamic>(
                "scud97_kssu.spGetFilmPlatforms",
                new { FilmID },
                "Default");
            return recs;
        }
        public async Task<List<Genres>> GetGenreList()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetGenreList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<Genres> wsl = new List<Genres>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new Genres();
            //    ws.GenreID = reader.GetInt32("GenreID");
            //    ws.GenreText = reader.GetString("GenreText");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //con.Close();
            //return (List<Genres>)wsl;
            var recs = await _dataAccess.LoadData<Genres, dynamic>(
                "scud97_kssu.spGetGenreList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<Directors>> GetDirectorsList()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetDirectorsList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<Directors> wsl = new List<Directors>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new Directors();
            //    ws.DirectorID = reader.GetInt32("DirectorID");
            //    ws.DirectorName = reader.GetString("DirectorName");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //con.Close();
            //return (List<Directors>)wsl;
            var recs = await _dataAccess.LoadData<Directors, dynamic>(
                "scud97_kssu.spGetDirectorsList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<Platforms>> GetPlatformsList()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetPlatformsList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<Platforms> wsl = new List<Platforms>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new Platforms();
            //    ws.PlatformID = reader.GetInt32("PlatformID");
            //    ws.PlatformText = reader.GetString("PlatformText");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //con.Close();
            //return (List<Platforms>)wsl;
            var recs = await _dataAccess.LoadData<Platforms, dynamic>(
                "scud97_kssu.spGetPlatformsList",
                new { },
                "Default");
            return recs;
        }
        public Task<int> UpdateFilm(int FilmID, int AddedByID, string FilmName, DateTime ReleaseDate, DateTime AddedDate, bool Watched, int Runtime)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spUpdateFilm";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmID", FilmID);
            //    cmd.Parameters.AddWithValue("@FilmName", FilmName);
            //    cmd.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
            //    cmd.Parameters.AddWithValue("@AddedDate", AddedDate);
            //    MySqlParameter paramWatched = new MySqlParameter
            //    {
            //        ParameterName = "@Watched",
            //        MySqlDbType = MySqlDbType.Bit,
            //        Value = Watched
            //    };
            //    cmd.Parameters.Add(paramWatched);
            //    cmd.Parameters.AddWithValue("@AddedByID", AddedByID);
            //    cmd.Parameters.AddWithValue("@Runtime", Runtime);
            //    cmd.ExecuteNonQuery();

            //    con.Close();
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("FilmID", FilmID);
            p.Add("FilmName", FilmName);
            p.Add("ReleaseDate", ReleaseDate);
            p.Add("AddedDate", AddedDate);
            p.Add("Watched", Watched);
            p.Add("AddedByID", AddedByID);
            p.Add("Runtime", Runtime);

            return _dataAccess.SaveData(
                "scud97_kssu.spUpdateFilm", 
                p,
                "Default");
        }
        public Task<int> ArchiveFilm(int FilmID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spArchiveFilm";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmID", FilmID);
            //    cmd.ExecuteNonQuery();

            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spArchiveFilm",
                new { FilmID },
                "Default");
        }
        public async Task<List<FilmList>> GetFilmList()
        {

            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spGetFilmList";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<FilmList> wsl = new List<FilmList>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new FilmList();
            //    ws.FilmID = reader.GetInt32("FilmID");
            //    ws.FilmName = reader.GetString("FilmName");
            //    ws.ReleaseDate = reader.GetString("ReleaseDate");
            //    ws.AddedDate = reader.GetString("AddedDate");
            //    ws.GenreText = reader.GetString("GenreText");
            //    ws.Watched = reader.GetString("Watched");
            //    ws.UserName = reader.GetString("UserName");
            //    ws.Runtime = reader.GetInt32("Runtime");
            //    ws.Platform = reader.GetString("PlatformText");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //return (List<FilmList>)wsl;
            var recs = await _dataAccess.LoadData<FilmList, dynamic>(
                "scud97_kssu.spGetFilmList",
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

        public async Task<int> AddFilm(string AddersName, DateTime AddedDate, string FilmName, DateTime ReleaseDate, bool Watched, int Runtime)
        {
            int AddedByID = 0;
            if (AddersName == "Kyle")
            {
                AddedByID = 1;
            }
            else { AddedByID = 2; }
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddFilm";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmName", FilmName);
            //    cmd.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
            //    cmd.Parameters.AddWithValue("@AddedDate", AddedDate);
            //    cmd.Parameters.AddWithValue("@Watched", Watched);
            //    cmd.Parameters.AddWithValue("@AddedByID", AddedByID);
            //    cmd.Parameters.AddWithValue("@Runtime", Runtime);
            //    cmd.Parameters.Add("@FilmID", MySqlDbType.Int32);
            //    cmd.Parameters["@FilmID"].Direction = ParameterDirection.Output; // from System.Data
            //    cmd.ExecuteNonQuery();
            //    Object obj = cmd.Parameters["@FilmID"].Value;
            //    var lParam = (Int32)obj;    // more useful datatype
            //    con.Close();
            //    return lParam;
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("FilmName", FilmName);
            p.Add("ReleaseDate", ReleaseDate);
            p.Add("AddedDate", AddedDate);
            p.Add("Watched", Watched);
            p.Add("AddedByID", AddedByID);
            p.Add("Runtime", Runtime);
            p.Add("FilmID", DbType.Int32, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spAddFilm", p, "Default");

            return p.Get<int>("FilmID");

        }
        public Task<int> AddFilmGenres(int FilmID, int GenreID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddFilmGenres";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmID", FilmID);
            //    cmd.Parameters.AddWithValue("@GenreID", GenreID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddFilmGenres",
                new { FilmID, GenreID },
                "Default");
        }
        public Task<int> AddFilmDirectors(int FilmID, int DirectorID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddFilmDirectors";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmID", FilmID);
            //    cmd.Parameters.AddWithValue("@DirectorID", DirectorID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddFilmDirectors",
                new { FilmID, DirectorID },
                "Default");
        }
        public Task<int> AddFilmPlatforms(int FilmID, int PlatformID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddFilmPlatforms";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmID", FilmID);
            //    cmd.Parameters.AddWithValue("@PlatformID", PlatformID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddFilmPlatforms",
                new { FilmID, PlatformID },
                "Default");
        }
        public Task<int> DeleteFilmGenre(int FilmGenreID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spDeleteFilmGenre";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmGenresID", FilmGenreID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteFilmGenre",
                new { FilmGenreID },
                "Default");
        }
        public Task<int> DeleteFilmDirector(int FilmDirectorID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spDeleteFilmDirector";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmDirectorsID", FilmDirectorID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteFilmDirector",
                new { FilmDirectorID },
                "Default");
        }
        public Task<int> DeleteFilmPlatform(int FilmPlatformID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spDeleteFilmPlatform";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmPlatformsID", FilmPlatformID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteFilmPlatform",
                new { FilmPlatformID },
                "Default");
        }
        public Task<int> AddGenre(string GenreText)
        {
            //using var con = new MySqlConnection(cs);
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
            //using var con = new MySqlConnection(cs);
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
        public async Task<bool> GenreDupeCheck(string GenreText)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spGenreDupeCheck";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@GenreText", GenreText);
            //    cmd.Parameters.Add("@GenreExists", MySqlDbType.Bool);
            //    cmd.Parameters["@GenreExists"].Direction = ParameterDirection.Output; // from System.Data
            //    cmd.ExecuteNonQuery();
            //    Object obj = cmd.Parameters["@GenreExists"].Value;
            //    var lParam = (Boolean)obj;    // more useful datatype

            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //    return lParam;
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("GenreText", GenreText);
            p.Add("GenreExists", DbType.Boolean, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spGenreDupeCheck", p, "Default");

            var i = p.Get<int>("GenreExists");
            return Convert.ToBoolean(i);
        }
        public async Task<bool> DirectorDupeCheck(string DirectorText)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spDirectorDupeCheck";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@DirectorText", DirectorText);
            //    cmd.Parameters.Add("@DirectorExists", MySqlDbType.Bool);
            //    cmd.Parameters["@DirectorExists"].Direction = ParameterDirection.Output; // from System.Data
            //    cmd.ExecuteNonQuery();
            //    Object obj = cmd.Parameters["@DirectorExists"].Value;
            //    var lParam = (Boolean)obj;    // more useful datatype

            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //    return lParam;
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("DirectorText", DirectorText);
            p.Add("DirectorExists", DbType.Boolean, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spDirectorDupeCheck", p, "Default");

            var i = p.Get<int>("DirectorExists");
            return Convert.ToBoolean(i);


        }
        public async Task<List<RandomFilm>> GetRandomFilm()
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //string sql = "CALL spRandomFilm";
            //using var cmd = new MySqlCommand(sql, con);
            //IList<RandomFilm> wsl = new List<RandomFilm>();
            //var reader = cmd.ExecuteReader();
            //while (reader.Read())
            //{
            //    var ws = new RandomFilm();
            //    ws.FilmID = reader.GetInt32("FilmID");
            //    ws.FilmName = reader.GetString("FilmName");
            //    ws.AddersName = reader.GetString("AddersName");
            //    ws.Runtime = reader.GetInt16("Runtime");
            //    wsl.Add(ws);
            //}
            //reader.Close();
            //return (List<RandomFilm>)wsl;
            var recs = await _dataAccess.LoadData<RandomFilm, dynamic>(
                "scud97_kssu.spRandomFilm",
                new { },
                "Default");
            return recs;
        }
        public Task<int> FilmWatched(int FilmID)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spFilmWatched";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@FilmID", FilmID);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spFilmWatched",
                new { FilmID },
                "Default");
        }
        public async Task<bool> PlatformDupeCheck(string PlatformText)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spPlatformDupeCheck";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@PlatformText", PlatformText);
            //    cmd.Parameters.Add("@PlatformExists", MySqlDbType.Bool);
            //    cmd.Parameters["@PlatformExists"].Direction = ParameterDirection.Output; // from System.Data
            //    cmd.ExecuteNonQuery();
            //    Object obj = cmd.Parameters["@PlatformExists"].Value;
            //    var lParam = (Boolean)obj;    // more useful datatype

            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //    return lParam;
            //}
            DynamicParameters p = new DynamicParameters();
            p.Add("PlatformText", PlatformText);
            p.Add("PlatformExists", DbType.Boolean, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spPlatformDupeCheck", p, "Default");

            var i = p.Get<int>("PlatformExists");
            return Convert.ToBoolean(i);
        }
        public Task<int> AddPlatform(string PlatformText)
        {
            //using var con = new MySqlConnection(cs);
            //con.Open();

            //using (MySqlCommand cmd = new MySqlCommand())
            //{
            //    cmd.Connection = con;
            //    cmd.CommandText = "spAddPlatform";
            //    cmd.CommandType = CommandType.StoredProcedure; // It is a Stored Proc

            //    cmd.Parameters.AddWithValue("@PlatformText", PlatformText);
            //    cmd.ExecuteNonQuery();
            //    con.Close();
            //}
            return _dataAccess.SaveData(
                "scud97_kssu.spAddPlatform",
                new { PlatformText },
                "Default");
        }
    }
}
