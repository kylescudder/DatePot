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
                var expenses = fd.GetExpenseList(cs);

                ActivityType = new List<SelectListItem>();
                Expense = new List<SelectListItem>();


                activitytypes.ForEach(x =>
                {
                    ActivityType.Add(new SelectListItem { Value = x.ActivityTypeID.ToString(), Text = x.ActivityType });
                });
                expenses.ForEach(x =>
                {
                    Expense.Add(new SelectListItem { Value = x.ExpenseID.ToString(), Text = x.ExpenseText });
                });


                RandomActivity = fd.GetRandomActivity(cs);

                return Page();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public ActionResult OnPost(string ActivityName, string Location, int? ExpenseID, string Description, bool Prebook, List<int> ActivityTypes)
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
                int ActivityID = fd.AddActivity(cs, ActivityName, Location, ExpenseID, Description, Prebook);
                
                foreach (var item in ActivityTypes)
                {
                    fd.AddActivityxType(cs, ActivityID, Convert.ToInt32(item));
                }

                result = new JsonResult(ActivityID);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        public async Task<IActionResult> OnPostActivityType()
        {
            try
            {
                string cs = _config.GetConnectionString("Default");
                if (!fd.ActivityTypeDupeCheck(cs, Request.Form["NewActivityType.ActivityType"].ToString()))
                {
                    fd.AddActivityType(cs, Request.Form["NewActivityType.ActivityType"].ToString());
                    return RedirectToPage("./Index");
                }
                return RedirectToPage("./Index", new { @redirect = "activitytypedupe", @value = Request.Form["NewActivityType.ActivityType"].ToString() });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
