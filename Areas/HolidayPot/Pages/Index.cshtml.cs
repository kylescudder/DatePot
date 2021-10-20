using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static DatePot.Areas.HolidayPot.Models.Holidays;
using DatePot.Areas.HolidayPot.Data;
using static DatePot.Models.Site;
using Microsoft.AspNetCore.Identity;
using DatePot.Areas.Identity.Data;
using DatePot.Data;
using Microsoft.AspNetCore.Http;

namespace DatePot.Areas.HolidayPot.Pages
{
    [ValidateAntiForgeryToken]
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly IConfiguration _config;
		private readonly IHolidayData _HolidayData;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IIdentityData _identityData;
		private readonly ISiteData _siteData;
		public IndexModel(ILogger<IndexModel> logger,
		IConfiguration config,
		IHolidayData HolidayData,
		UserManager<IdentityUser> userManager,
		IIdentityData identityData,
		ISiteData siteData)
		{
			_logger = logger;
			_config = config;
			_HolidayData = HolidayData;
			_userManager = userManager;
			_identityData = identityData;
			_siteData = siteData;
		}
		public List<HolidayList> Holidays { get; set; }
		public NewHoliday NewHoliday { get; set; }
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
				int index = PotAccess.FindIndex(item => item.PotID == 7);
				if (index == -1)
				{
					return RedirectToPage("/Account/AccessDenied", new { area = "Identity" });
				}
				Holidays = _HolidayData.GetHolidayList(UserGroupID).Result;

				return Page();
			}
			catch (Exception ex)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(ex.ToString());
			}
		}
		public async Task<JsonResult> OnPost(string Country, string City, bool Been, DateTime? DateBeen)
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
				int HolidayID = _HolidayData.AddHoliday(Country, City, Been, DateBeen, UserGroupID).Result;

				result = new JsonResult(HolidayID);
				return result;
			}
			catch (Exception ex)
			{
				SentrySdk.CaptureException(ex);
				throw new Exception(ex.ToString());
			}
		}
	}
}
