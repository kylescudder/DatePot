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
using static DatePot.Areas.ActivityPot.Models.Activitys;
using DatePot.Areas.ActivityPot.Data;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Http;

namespace DatePot.Areas.ActivityPot.Pages
{
	[ValidateAntiForgeryToken]
	public class ViewModel : PageModel
	{
		private readonly IConfiguration _config;
		private readonly IActivityData _activityData;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		public ViewModel(IConfiguration config,
		IActivityData activityData,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData)
		{
			_config = config;
			_activityData = activityData;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
		}
		[BindProperty(SupportsGet = true)]
		public int Id { get; set; }
		[BindProperty]
		public UpdateActivityDetails UpdateActivityDetails { get; set; }

		public ActivityDetails ActivityDetails { get; set; }
		//public string Genre { get; set; }
		public List<SelectListItem> ActivityTypes { get; set; }
		public List<ActivityxTypes> ActivityxTypes { get; set; }
		public List<SelectListItem> Expenses { get; set; }
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
				PotAccess = await _siteData.GetPotAccess(user.Id.ToString());
				int index = PotAccess.FindIndex(item => item.PotID == 3);
				if (index == -1)
				{
					return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
				}
				ActivityDetails = _activityData.GetActivityDetails(Id).Result.FirstOrDefault();
				if (ActivityDetails.UserGroupID != UserGroupID)
                {
                    return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
                }
				if (ActivityDetails != null)
				{
					if (ActivityDetails != null)
					{
						var expenses = _activityData.GetExpenseList();
						var activitytypes = _activityData.GetyActivityTypeList();

						Expenses = new List<SelectListItem>();
						ActivityTypes = new List<SelectListItem>();
						ActivityxTypes = new List<ActivityxTypes>();

						ActivityxTypes = _activityData.GetActivityxTypes(Id).Result;

						expenses.Result.ForEach(x =>
						{
							if (ActivityDetails.ExpenseID == x.ExpenseID)
							{
								Expenses.Add(new SelectListItem { Value = x.ExpenseID.ToString(), Text = x.ExpenseText, Selected = true });
							}
							else
							{
								Expenses.Add(new SelectListItem { Value = x.ExpenseID.ToString(), Text = x.ExpenseText });
							}
						});
						activitytypes.Result.ForEach(x =>
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
		public async Task<IActionResult> OnPost(int ActivityID, string ActivityName, string Location, int ExpenseID, string Description, bool Prebook, List<int> ActivityTypes)
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
				await _activityData.UpdateActivity(ActivityID, ActivityName, Location, ExpenseID, Description, Prebook);

				foreach (var item in ActivityTypes)
				{
					if (item != 0)
					{
						await _activityData.AddActivityxType(ActivityID, Convert.ToInt32(item));
					}

				}

				result = new JsonResult(ActivityID);
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
				await _activityData.ArchiveActivity(UpdateActivityDetails.ActivityID);

				return RedirectToPage("./Index");
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPostDeleteActivityType(int ActivityxTypeID, int ActivityID)
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
				await _activityData.DeleteActivityxType(ActivityxTypeID);

				result = new JsonResult(ActivityID);
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.ToString());
			}
		}
	}
}
