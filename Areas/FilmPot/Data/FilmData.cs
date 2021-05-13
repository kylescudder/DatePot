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
using static DatePot.Models.Site;

namespace DatePot.Areas.FilmPot.Data
{
    public class FilmData : IFilmData
    {
        private readonly ISqlDb _dataAccess;
        public FilmData(ISqlDb dataAccess
            )
        {
            _dataAccess = dataAccess;
        }
        public async Task<List<FilmDetails>> GetFilmDetails(int FilmID)
        {
            var recs = await _dataAccess.LoadData<FilmDetails, dynamic>(
                "scud97_kssu.spGetFilmDetails",
                new { FilmID },
                "Default");
            return recs;

        }
        public async Task<List<FilmGenres>> GetFilmGenres(int FilmID)
        {
            var recs = await _dataAccess.LoadData<FilmGenres, dynamic>(
                "scud97_kssu.spGetFilmGenres",
                new { FilmID },
                "Default");
            return recs;

        }
        public async Task<List<FilmDirectors>> GetFilmDirectors(int FilmID)
        {
            var recs = await _dataAccess.LoadData<FilmDirectors, dynamic>(
                "scud97_kssu.spGetFilmDirectors",
                new { FilmID },
                "Default");
            return recs;

        }
        public async Task<List<FilmPlatforms>> GetFilmPlatforms(int FilmID)
        {
            var recs = await _dataAccess.LoadData<FilmPlatforms, dynamic>(
                "scud97_kssu.spGetFilmPlatforms",
                new { FilmID },
                "Default");
            return recs;
        }
        public async Task<List<Genres>> GetGenreList()
        {
            var recs = await _dataAccess.LoadData<Genres, dynamic>(
                "scud97_kssu.spGetGenreList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<Directors>> GetDirectorsList()
        {
            var recs = await _dataAccess.LoadData<Directors, dynamic>(
                "scud97_kssu.spGetDirectorsList",
                new { },
                "Default");
            return recs;
        }
        public async Task<List<Platforms>> GetPlatformsList()
        {
            var recs = await _dataAccess.LoadData<Platforms, dynamic>(
                "scud97_kssu.spGetPlatformsList",
                new { },
                "Default");
            return recs;
        }
        public Task<int> UpdateFilm(int FilmID, int AddedByID, string FilmName, DateTime ReleaseDate, DateTime AddedDate, bool Watched, int Runtime)
        {
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
            return _dataAccess.SaveData(
                "scud97_kssu.spArchiveFilm",
                new { FilmID },
                "Default");
        }
        public async Task<List<FilmList>> GetFilmList()
        {
            var recs = await _dataAccess.LoadData<FilmList, dynamic>(
                "scud97_kssu.spGetFilmList",
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

        public async Task<int> AddFilm(string AddersName, DateTime AddedDate, string FilmName, DateTime ReleaseDate, bool Watched, int Runtime)
        {
            int AddedByID = 0;
            if (AddersName == "Kyle")
            {
                AddedByID = 1;
            }
            else { AddedByID = 2; }

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
            return _dataAccess.SaveData(
                "scud97_kssu.spAddFilmGenres",
                new { FilmID, GenreID },
                "Default");
        }
        public Task<int> AddFilmDirectors(int FilmID, int DirectorID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spAddFilmDirectors",
                new { FilmID, DirectorID },
                "Default");
        }
        public Task<int> AddFilmPlatforms(int FilmID, int PlatformID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spAddFilmPlatforms",
                new { FilmID, PlatformID },
                "Default");
        }
        public Task<int> DeleteFilmGenre(int FilmGenreID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteFilmGenre",
                new { FilmGenreID },
                "Default");
        }
        public Task<int> DeleteFilmDirector(int FilmDirectorID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteFilmDirector",
                new { FilmDirectorID },
                "Default");
        }
        public Task<int> DeleteFilmPlatform(int FilmPlatformID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spDeleteFilmPlatform",
                new { FilmPlatformID },
                "Default");
        }
        public Task<int> AddGenre(string GenreText)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spAddGenre",
                new { GenreText },
                "Default");
        }
        public Task<int> AddDirector(string DirectorText)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spAddDirector",
                new { DirectorText },
                "Default");
        }
        public async Task<bool> GenreDupeCheck(string GenreText)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("GenreText", GenreText);
            p.Add("GenreExists", DbType.Boolean, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spGenreDupeCheck", p, "Default");

            var i = p.Get<int>("GenreExists");
            return Convert.ToBoolean(i);
        }
        public async Task<bool> DirectorDupeCheck(string DirectorText)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("DirectorText", DirectorText);
            p.Add("DirectorExists", DbType.Boolean, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spDirectorDupeCheck", p, "Default");

            var i = p.Get<int>("DirectorExists");
            return Convert.ToBoolean(i);


        }
        public async Task<List<RandomFilm>> GetRandomFilm()
        {
            var recs = await _dataAccess.LoadData<RandomFilm, dynamic>(
                "scud97_kssu.spRandomFilm",
                new { },
                "Default");
            return recs;
        }
        public Task<int> FilmWatched(int FilmID)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spFilmWatched",
                new { FilmID },
                "Default");
        }
        public async Task<bool> PlatformDupeCheck(string PlatformText)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("PlatformText", PlatformText);
            p.Add("PlatformExists", DbType.Boolean, direction: ParameterDirection.Output);

            await _dataAccess.SaveData("scud97_kssu.spPlatformDupeCheck", p, "Default");

            var i = p.Get<int>("PlatformExists");
            return Convert.ToBoolean(i);
        }
        public Task<int> AddPlatform(string PlatformText)
        {
            return _dataAccess.SaveData(
                "scud97_kssu.spAddPlatform",
                new { PlatformText },
                "Default");
        }
    }
}
