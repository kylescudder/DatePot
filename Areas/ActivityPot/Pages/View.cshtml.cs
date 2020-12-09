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
using MySql.Data.MySqlClient;
using static DatePot.Areas.ActivityPot.Models.Activitys;
using DatePot.Areas.ActivityPot.Data;

namespace DatePot.Areas.ActivityPot.Pages
{
    public class ViewModel : PageModel
    {
        private readonly IConfiguration _config;
        public ViewModel(IConfiguration config)
        {
            _config = config;
        }
        ActivityData fd = new ActivityData();
        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }
        [BindProperty]
        public UpdateActivityDetails UpdateActivityDetails { get; set; }

        public ActivityDetails ActivityDetails { get; set; }
        //public string Genre { get; set; }
        public List<SelectListItem> ActivityTypes { get; set; }
        public async Task<IActionResult> OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            try
            {
                string cs = _config.GetConnectionString("Default");
                ActivityDetails = fd.GetActivityDetails(cs, Id).FirstOrDefault();
                if (ActivityDetails != null)
                {
                    if (ActivityDetails != null)
                    {
                        var activitytypes = fd.GetyActivityTypeList(cs);

                        //Genre = genres.Where(x => x.GenreID == ActivityDetails.GenreID).FirstOrDefault()?.GenreText;

                        ActivityTypes = new List<SelectListItem>();

                        activitytypes.ForEach(x =>
                        {
                            ActivityTypes.Add(new SelectListItem { Value = x.ActivityTypeID.ToString(), Text = x.ActivityType });
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
        public async Task<IActionResult> OnPost()
        {
            try
            {
                if (ModelState.IsValid == false)
                {
                    return Page();
                }
                string cs = _config.GetConnectionString("Default");
                fd.UpdateActivity(cs, UpdateActivityDetails);

                return RedirectToPage("./View", new { @Id = UpdateActivityDetails.ActivityID, @redirect = "update" });
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
                fd.ArchiveActivity(cs, UpdateActivityDetails.ActivityID);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
