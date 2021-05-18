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
using static DatePot.Areas.ActivityPot.Models.Activitys;
using DatePot.Areas.ActivityPot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using Microsoft.AspNetCore.Http;

namespace DatePot.Areas.ActivityPot.Pages
{
	[ValidateAntiForgeryToken]
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IConfiguration _config;
		private readonly IActivityData _activityData;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		public IndexModel(ILogger<IndexModel> logger,
		IConfiguration config,
		IActivityData activityData,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData)
		{
			_logger = logger;
			_config = config;
			_activityData = activityData;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
		}
		public List<ActivityList> Activitys { get; set; }
		public List<SelectListItem> Expense { get; set; }
		public List<SelectListItem> ActivityType { get; set; }
		[BindProperty]
		public NewActivity NewActivity { get; set; }
		public NewActivityType NewActivityType { get; set; }
		public List<RandomActivity> RandomActivity { get; set; }
		public List<PotAccess> PotAccess { get; set; }
		public async Task<ActionResult> OnGet()
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
				int index = PotAccess.FindIndex(item => item.PotID == 3);
				if (index == -1)
				{
					return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
				}
				Activitys = _activityData.GetActivityList(UserGroupID).Result;
				var activitytypes = _activityData.GetyActivityTypeList();
				var expenses = _activityData.GetExpenseList();

				ActivityType = new List<SelectListItem>();
				Expense = new List<SelectListItem>();


				activitytypes.Result.ForEach(x =>
				{
					ActivityType.Add(new SelectListItem { Value = x.ActivityTypeID.ToString(), Text = x.ActivityType });
				});
				expenses.Result.ForEach(x =>
				{
					Expense.Add(new SelectListItem { Value = x.ExpenseID.ToString(), Text = x.ExpenseText });
				});


				RandomActivity = _activityData.GetRandomActivity(UserGroupID).Result;

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
				int? UserGroupID = HttpContext.Session.GetInt32("UserGroupID");
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
				int ActivityID = _activityData.AddActivity(ActivityName, Location, ExpenseID, Description, Prebook, UserGroupID).Result;

				foreach (var item in ActivityTypes)
				{
					_activityData.AddActivityxType(ActivityID, Convert.ToInt32(item));
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
				if (!_activityData.ActivityTypeDupeCheck(Request.Form["NewActivityType.ActivityType"].ToString()).Result)
				{
					await _activityData.AddActivityType(Request.Form["NewActivityType.ActivityType"].ToString());
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
