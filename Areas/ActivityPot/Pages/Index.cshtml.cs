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
using static DatePot.Areas.ActivityPot.Models.Activitys;
using DatePot.Areas.ActivityPot.Data;

namespace DatePot.Areas.ActivityPot.Pages
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
        ActivityData fd = new ActivityData();
        public List<ActivityList> Activitys { get; set; }
        public List<SelectListItem> Expense { get; set; }
        public List<SelectListItem> ActivityType { get; set; }
        [BindProperty]
        public NewActivity NewActivity { get; set; }
        public NewActivityType NewActivityType { get; set; }
        public List<RandomActivity> RandomActivity { get; set; }
        public ActionResult OnGet()
         {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
                string cs = _config.GetConnectionString("Default");
                Activitys = fd.GetActivityList(cs);
                var activitytypes = fd.GetyActivityTypeList(cs);

                ActivityType = new List<SelectListItem>();

                activitytypes.ForEach(x =>
                {
                    ActivityType.Add(new SelectListItem { Value = x.ActivityTypeID.ToString(), Text = x.ActivityType });
                });

                RandomActivity = fd.GetRandomActivity(cs);

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
                int ActivityID = fd.AddActivity(cs, NewActivity);

                return RedirectToPage("./View", new { Id = ActivityID });
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
        public async Task<IActionResult> OnPostAddActivity()
        {
            try
            {
                string cs = _config.GetConnectionString("Default");

                fd.ActivityWatched(cs, Convert.ToInt32(Request.Form["ActivityID"]));

                //return RedirectToPage("./Index", new { @redirect = "directordupe", @value = Request.Form["NewDirector.DirectorText"].ToString() });

                return RedirectToPage("./Index", new { @redirect = "ActivityWatched" });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
