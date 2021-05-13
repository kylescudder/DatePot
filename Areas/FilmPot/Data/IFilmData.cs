using System;
using System.Collections.Generic;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using static DatePot.Areas.FilmPot.Models.Films;
using static DatePot.Models.Site;

namespace DatePot.Areas.FilmPot.Data
{
    public interface IFilmData
    {
        Task<int> AddDirector(string DirectorText);
        Task<int> AddFilm(string AddersName, DateTime AddedDate, string FilmName, DateTime ReleaseDate, bool Watched, int Runtime);
        Task<int> AddFilmDirectors(int FilmID, int DirectorID);
        Task<int> AddFilmGenres(int FilmID, int GenreID);
        Task<int> AddFilmPlatforms(int FilmID, int PlatformID);
        Task<int> AddGenre(string GenreText);
        Task<int> AddPlatform(string PlatformText);
        Task<int> ArchiveFilm(int FilmID);
        Task<int> DeleteFilmDirector(int FilmDirectorID);
        Task<int> DeleteFilmGenre(int FilmGenreID);
        Task<int> DeleteFilmPlatform(int FilmPlatformID);
        Task<bool> DirectorDupeCheck(string DirectorText);
        Task<int> FilmWatched(int FilmID);
        Task<bool> GenreDupeCheck(string GenreText);
        Task<List<Directors>> GetDirectorsList();
        Task<List<FilmDetails>> GetFilmDetails(int FilmID);
        Task<List<FilmDirectors>> GetFilmDirectors(int FilmID);
        Task<List<FilmGenres>> GetFilmGenres(int FilmID);
        Task<List<FilmList>> GetFilmList();
        Task<List<FilmPlatforms>> GetFilmPlatforms(int FilmID);
        Task<List<Genres>> GetGenreList();
        Task<List<Platforms>> GetPlatformsList();
        Task<List<RandomFilm>> GetRandomFilm();
        Task<List<UserList>> GetUserList();
        Task<bool> PlatformDupeCheck(string PlatformText);
        Task<int> UpdateFilm(int FilmID, int AddedByID, string FilmName, DateTime ReleaseDate, DateTime AddedDate, bool Watched, int Runtime);
    }
}
