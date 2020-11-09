using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using DatePot.Data;
using DatePot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using static DatePot.Models.Films;
using static DatePot.Data.FilmData;

namespace DatePot.Areas.FilmPot.Pages
{
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
        public List<SelectListItem> Genre { get; set; }
        [BindProperty]
        public NewFilm NewFilm { get; set; }
        public NewGenre NewGenre { get; set; }
        public NewDirector NewDirector { get; set; }
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
                var genres = fd.GetGenreList(cs);

                Genre = new List<SelectListItem>();

                genres.ForEach(x =>
                {
                    Genre.Add(new SelectListItem { Value = x.GenreID.ToString(), Text = x.GenreText });
                });

                RandomFilm = fd.GetRandomFilm(cs);

                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public ActionResult OnPost()
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return Page();
                }
                string cs = _config.GetConnectionString("Default");
                int FilmID = fd.AddFilm(cs, NewFilm);

                return RedirectToPage("./View", new { Id = FilmID });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
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
