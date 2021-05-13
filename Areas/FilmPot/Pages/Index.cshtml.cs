using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static DatePot.Areas.FilmPot.Models.Films;
using DatePot.Areas.FilmPot.Data;
using static DatePot.Models.Site;
using DatePot.Data;
using DatePot.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace DatePot.Areas.FilmPot.Pages
{
    [ValidateAntiForgeryToken]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _config;
        private readonly IFilmData _filmData;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IIdentityData _identityData;
        private readonly ISiteData _siteData;
        public IndexModel(ILogger<IndexModel> logger, 
        IConfiguration config, 
        IFilmData filmData,
        UserManager<IdentityUser> userManager,
        IIdentityData identityData,
        ISiteData siteData)
        {
            _logger = logger;
            _config = config;
            _filmData = filmData;
            _userManager = userManager;
            _identityData = identityData;
            _siteData = siteData;
        }
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
        public List<PotAccess> PotAccess { get; set; }
        public async Task<ActionResult> OnGet()
         {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
				var user = await _userManager.GetUserAsync(User);
				var UserID = _identityData.GetUser(user.Id.ToString()).Result.FirstOrDefault().UserID.ToString();
				PotAccess = await _siteData.GetPotAccess(Convert.ToInt32(UserID));
				int index = PotAccess.FindIndex(item => item.PotID == 1);
                if (index == -1)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
                Films = _filmData.GetFilmList().Result;
                Users = _filmData.GetUserList().Result;

                var genres = _filmData.GetGenreList();
                var directors = _filmData.GetDirectorsList();
                var platforms = _filmData.GetPlatformsList();

                Genre = new List<SelectListItem>();
                Director = new List<SelectListItem>();
                Platform = new List<SelectListItem>();

                genres.Result.ForEach(x =>
                {
                    Genre.Add(new SelectListItem { Value = x.GenreID.ToString(), Text = x.GenreText });
                });
                directors.Result.ForEach(x =>
                {
                    Director.Add(new SelectListItem { Value = x.DirectorID.ToString(), Text = x.DirectorName });
                });
                platforms.Result.ForEach(x =>
                {
                    Platform.Add(new SelectListItem { Value = x.PlatformID.ToString(), Text = x.PlatformText });
                });

                RandomFilm = _filmData.GetRandomFilm().Result;


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
                int FilmID = await _filmData.AddFilm(AddersName, AddedDate, FilmName, ReleaseDate, Watched, Runtime);
                foreach (var item in Genres)
                {
                    await _filmData.AddFilmGenres(FilmID, Convert.ToInt32(item));

                }
                foreach (var item in Directors)
                {
                    await _filmData.AddFilmDirectors(FilmID, Convert.ToInt32(item));

                }
                foreach (var item in Platforms)
                {
                    await _filmData.AddFilmPlatforms(FilmID, Convert.ToInt32(item));

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
                if (!_filmData.GenreDupeCheck(Request.Form["NewGenre.GenreText"].ToString()).Result)
                {
                    await _filmData.AddGenre(Request.Form["NewGenre.GenreText"].ToString());
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
                if (!_filmData.DirectorDupeCheck(Request.Form["NewDirector.DirectorText"].ToString()).Result)
                {
                    await _filmData.AddDirector(Request.Form["NewDirector.DirectorText"].ToString());
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
                if (!_filmData.PlatformDupeCheck(Request.Form["NewPlatform.PlatformText"].ToString()).Result)
                {
                    await _filmData.AddPlatform(Request.Form["NewPlatform.PlatformText"].ToString());
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
                await _filmData.FilmWatched(Convert.ToInt32(Request.Form["FilmID"]));

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
