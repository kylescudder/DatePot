using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using static DatePot.Areas.FilmPot.Models.Films;
using DatePot.Areas.FilmPot.Data;

namespace DatePot.Areas.FilmPot.Pages
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        public IndexModel(ILogger<IndexModel> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        FilmData fd = new FilmData();
        public List<FilmList> Films { get; set; }
        public List<UserList> Users { get; set; }
        public List<SelectListItem> Genre { get; set; }
        public List<SelectListItem> Director { get; set; }
        public List<SelectListItem> Platform { get; set; }
        [BindProperty]
        public NewFilm NewFilm { get; set; }
        public NewGenre NewGenre { get; set; }
        public NewDirector NewDirector { get; set; }
        public NewPlatform NewPlatform { get; set; }
        public List<RandomFilm> RandomFilm { get; set; }
        public ActionResult OnGet()
         {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
                string cs = _config.GetConnectionString("Default");
                Films = fd.GetFilmList(cs);
                Users = fd.GetUserList(cs);
                var genres = fd.GetGenreList(cs);
                var directors = fd.GetDirectorsList(cs);
                var platforms = fd.GetPlatformsList(cs);

                Genre = new List<SelectListItem>();
                Director = new List<SelectListItem>();
                Platform = new List<SelectListItem>();

                genres.ForEach(x =>
                {
                    Genre.Add(new SelectListItem { Value = x.GenreID.ToString(), Text = x.GenreText });
                });
                directors.ForEach(x =>
                {
                    Director.Add(new SelectListItem { Value = x.DirectorID.ToString(), Text = x.DirectorName });
                });
                platforms.ForEach(x =>
                {
                    Platform.Add(new SelectListItem { Value = x.PlatformID.ToString(), Text = x.PlatformText });
                });

                RandomFilm = fd.GetRandomFilm(cs);

                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<JsonResult> OnPost(string AddersName, DateTime AddedDate, string FilmName, DateTime ReleaseDate, bool Watched, int Runtime, List<int>Genres, List<int> Directors, List<int> Platforms)
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
                int FilmID = fd.AddFilm(cs, AddersName, AddedDate, FilmName, ReleaseDate, Watched, Runtime);
                foreach (var item in Genres)
                {
                    fd.AddFilmGenres(cs, FilmID, Convert.ToInt32(item));

                }
                foreach (var item in Directors)
                {
                    fd.AddFilmDirectors(cs, FilmID, Convert.ToInt32(item));

                }
                foreach (var item in Platforms)
                {
                    fd.AddFilmPlatforms(cs, FilmID, Convert.ToInt32(item));

                }
                result = new JsonResult(FilmID);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured");
                JsonResult result = null;
                result = new JsonResult(ex);
                return result;
            }
        }
        public async Task<IActionResult> OnPostGenre()
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                if (!fd.GenreDupeCheck(cs, Request.Form["NewGenre.GenreText"].ToString()))
                {
                    fd.AddGenre(cs, Request.Form["NewGenre.GenreText"].ToString());
                    return RedirectToPage("./Index");
                }
                return RedirectToPage("./Index", new { @redirect = "genredupe", @value = Request.Form["NewGenre.GenreText"].ToString() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<IActionResult> OnPostDirector()
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                if (!fd.DirectorDupeCheck(cs, Request.Form["NewDirector.DirectorText"].ToString()))
                {
                    fd.AddDirector(cs, Request.Form["NewDirector.DirectorText"].ToString());
                    return RedirectToPage("./Index");
                }
                return RedirectToPage("./Index", new { @redirect = "directordupe", @value = Request.Form["NewDirector.DirectorText"].ToString() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<IActionResult> OnPostPlatform()
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                if (!fd.PlatformDupeCheck(cs, Request.Form["NewPlatform.PlatformText"].ToString()))
                {
                    fd.AddPlatform(cs, Request.Form["NewPlatform.PlatformText"].ToString());
                    return RedirectToPage("./Index");
                }
                return RedirectToPage("./Index", new { @redirect = "Platformdupe", @value = Request.Form["NewPlatform.PlatformText"].ToString() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public async Task<IActionResult> OnPostAddFilm()
        {
            try
            {
                string cs = _config.GetConnectionString("Default");

                fd.FilmWatched(cs, Convert.ToInt32(Request.Form["FilmID"]));

                //return RedirectToPage("./Index", new { @redirect = "directordupe", @value = Request.Form["NewDirector.DirectorText"].ToString() });

                return RedirectToPage("./Index", new { @redirect = "FilmWatched" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
