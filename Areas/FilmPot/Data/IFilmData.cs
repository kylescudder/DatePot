using DatePot.Areas.FilmPot.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<List<Films.Directors>> GetDirectorsList();
        Task<List<Films.FilmDetails>> GetFilmDetails(int FilmID);
        Task<List<Films.FilmDirectors>> GetFilmDirectors(int FilmID);
        Task<List<Films.FilmGenres>> GetFilmGenres(int FilmID);
        Task<List<Films.FilmList>> GetFilmList();
        Task<List<Films.FilmPlatforms>> GetFilmPlatforms(int FilmID);
        Task<List<Films.Genres>> GetGenreList();
        Task<List<Films.Platforms>> GetPlatformsList();
        Task<List<Films.RandomFilm>> GetRandomFilm();
        Task<List<Films.UserList>> GetUserList();
        Task<bool> PlatformDupeCheck(string PlatformText);
        Task<int> UpdateFilm(int FilmID, int AddedByID, string FilmName, DateTime ReleaseDate, DateTime AddedDate, bool Watched, int Runtime);
    }
}