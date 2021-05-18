using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
//using MySql.Data.MySqlClient;
using static DatePot.Areas.FilmPot.Models.Films;
using DatePot.Areas.FilmPot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using Microsoft.AspNetCore.Http;

namespace DatePot.Areas.FilmPot.Pages
{
    [ValidateAntiForgeryToken]
    public class ViewModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly IFilmData _filmData;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityData _identityData;
        private readonly ISiteData _siteData;
        public ViewModel(IConfiguration config, 
        IFilmData filmData,
        UserManager<IdentityUser> userManager,
        IIdentityData identityData,
        ISiteData siteData)
        {
            _config = config;
            _filmData = filmData;
            _userManager = userManager;
            _identityData = identityData;
            _siteData = siteData;
        }
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public UpdateFilmDetails UpdateFilmDetails { get; set; }

        public FilmDetails FilmDetails { get; set; }
        public List<FilmGenres> FilmGenres { get; set; }
        public List<FilmDirectors> FilmDirectors { get; set; }
        public List<FilmPlatforms> FilmPlatforms { get; set; }
        //public string Genre { get; set; }
        public List<SelectListItem> Genres { get; set; }
        public List<SelectListItem> Directors { get; set; }
        public List<SelectListItem> Platforms { get; set; }
        public List<SelectListItem> Users { get; set; }
        public List<PotAccess> PotAccess { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
                int? UserGroupID = HttpContext.Session.GetInt32("UserGroupID");
				var user = await _userManager.GetUserAsync(User);
				PotAccess = await _siteData.GetPotAccess(user.Id.ToString(), UserGroupID);
				int index = PotAccess.FindIndex(item => item.PotID == 1);
                if (index == -1)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
                string cs = _config.GetConnectionString("Default");
                FilmDetails = _filmData.GetFilmDetails(Id).Result.FirstOrDefault();
                if (FilmDetails.UserGroupID != UserGroupID)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
                if (FilmDetails != null)
                {
                    if (FilmDetails != null)
                    {
                        var genres = _filmData.GetGenreList();
                        var directors = _filmData.GetDirectorsList();
                        var platforms = _filmData.GetPlatformsList();
                        var users = _filmData.GetUserList(UserGroupID);

                        //Genre = genres.Where(x => x.GenreID == FilmDetails.GenreID).FirstOrDefault()?.GenreText;

                        Genres = new List<SelectListItem>();
                        Directors = new List<SelectListItem>();
                        Platforms = new List<SelectListItem>();
                        Users = new List<SelectListItem>();
                        FilmGenres = new List<FilmGenres>();
                        FilmDirectors = new List<FilmDirectors>();
                        FilmPlatforms = new List<FilmPlatforms>();

                        FilmGenres = _filmData.GetFilmGenres(Id).Result;
                        FilmDirectors = _filmData.GetFilmDirectors(Id).Result;
                        FilmPlatforms = _filmData.GetFilmPlatforms(Id).Result;

                        genres.Result.ForEach(x =>
                        {
                            Genres.Add(new SelectListItem { Value = x.GenreID.ToString(), Text = x.GenreText });
                        });
                        directors.Result.ForEach(x =>
                        {
                            Directors.Add(new SelectListItem { Value = x.DirectorID.ToString(), Text = x.DirectorName });
                        });
                        platforms.Result.ForEach(x =>
                        {
                            Platforms.Add(new SelectListItem { Value = x.PlatformID.ToString(), Text = x.PlatformText });
                        });
                        users.Result.ForEach(x =>
                        {
                            if (FilmDetails.AddedByID == x.UserID)
                            {
                                Users.Add(new SelectListItem { Value = x.UserID.ToString(), Text = x.UserName, Selected = true });
                            }
                            else
                            {
                                Users.Add(new SelectListItem { Value = x.UserID.ToString(), Text = x.UserName });
                            }
                        });
                    }
                }

                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<JsonResult> OnPost(int FilmID, int AddedByID, string FilmName, DateTime ReleaseDate, DateTime AddedDate, bool Watched, int Runtime, List<int> Genre, List<int> Director, List<int> Platform)
        {
            try
            {
                JsonResult result = null;
                if (ModelState.IsValid == false)
                {
                    foreach (var modelStateKey in ViewData.ModelState.Keys)
                    {
                        var value = ViewData.ModelState[modelStateKey];
                        foreach (var error in value.Errors)
                        {
                            var errorMessage = error.ErrorMessage;
                            result = new JsonResult(modelStateKey + ": " + errorMessage);
                        }
                    }
                    return result;
                }
                string cs = _config.GetConnectionString("Default");
                await _filmData.UpdateFilm(FilmID, AddedByID, FilmName, ReleaseDate, AddedDate, Watched, Runtime);

                foreach (var item in Genre)
                {
                    if (item != 0)
                    {
                        await _filmData.AddFilmGenres(FilmID, Convert.ToInt32(item));
                    }

                }
                foreach (var item in Director)
                {
                    if (item != 0)
                    {
                        await _filmData.AddFilmDirectors(FilmID, Convert.ToInt32(item));
                    }

                }
                foreach (var item in Platform)
                {
                    if (item != 0)
                    {
                        await _filmData.AddFilmPlatforms(FilmID, Convert.ToInt32(item));
                    }

                }

                result = new JsonResult(FilmID);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<JsonResult> OnPostDeleteGenre(int FilmGenreID, int FilmID)
        {
            try
            {
                JsonResult result = null;
                if (ModelState.IsValid == false)
                {
                    foreach (var modelStateKey in ViewData.ModelState.Keys)
                    {
                        var value = ViewData.ModelState[modelStateKey];
                        foreach (var error in value.Errors)
                        {
                            var errorMessage = error.ErrorMessage;
                            result = new JsonResult(modelStateKey + ": " + errorMessage);
                        }
                    }
                    return result;
                }
                string cs = _config.GetConnectionString("Default");
                await _filmData.DeleteFilmGenre(FilmGenreID);

                result = new JsonResult(FilmID);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<JsonResult> OnPostDeleteDirector(int FilmDirectorID, int FilmID)
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                await _filmData.DeleteFilmDirector(FilmDirectorID);

                JsonResult result = null;
                result = new JsonResult(FilmID);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<JsonResult> OnPostDeletePlatform(int FilmPlatformID, int FilmID)
        {
            try
            {
                JsonResult result = null;
                if (ModelState.IsValid == false)
                {
                    foreach (var modelStateKey in ViewData.ModelState.Keys)
                    {
                        var value = ViewData.ModelState[modelStateKey];
                        foreach (var error in value.Errors)
                        {
                            var errorMessage = error.ErrorMessage;
                            result = new JsonResult(modelStateKey + ": " + errorMessage);
                        }
                    }
                    return result;
                }
                string cs = _config.GetConnectionString("Default");
                await _filmData.DeleteFilmPlatform(FilmPlatformID);

                result = new JsonResult(FilmID);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<IActionResult> OnPostArchive()
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return Page();
                }
                string cs = _config.GetConnectionString("Default");
                await _filmData.ArchiveFilm(UpdateFilmDetails.FilmID);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
